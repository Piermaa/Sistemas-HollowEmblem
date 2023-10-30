using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;
//TODO: Remover crouch
public class PlayerMovementController : MonoBehaviour, IMovable
{
	#region Class Properties

	public bool MustSlam => _mustSlam;
	public bool CanMove { get { return _canMove; } set { _canMove = value; } }
	
	#region Class Serialized Properties

	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Rigidbody2D m_Rigidbody2D;

	[Header("Events")]	[Space]
	[SerializeField] private UnityEvent OnLandEvent;
	[SerializeField] public UnityEvent OnSlamEvent;
	#endregion
	
	private const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool _isGrounded;            // Whether or not the player is grounded.
	private bool _isFacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 _velocity = Vector3.zero;
	private bool _mustSlam;
	private bool _canDoubleJump;
	private bool _canMove = true;
	private Player _player;

	private Animator _animator;
	private bool _jumpTransitionDelayOn=false;
	private const string FALLING_ANIMATOR_PARAMETER = "Falling";
	private const string IDLE_ANIMATOR_PARAMETER = "Idle";
	private const string JUMP_ANIMATOR_PARAMETER = "Jump";
	private const string DOUBLE_JUMPING_ANIMATOR_PARAMETER = "DoubleJumping";
	private const string DOUBLE_JUMP_ANIMATOR_PARAMETER = "DoubleJump";
	private const string RUN_ANIMATOR_PARAMETER = "Run";
	#endregion
    
	#region Monobehaviour Callbacks

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		_player = GetComponent<Player>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = _isGrounded;
		_isGrounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				if (colliders[i].TryGetComponent<Platform>(out var plat))
				{
					TryGetComponent<Collider2D>(out var col);
					plat.PlayerSnap(col);
				}

				if (!wasGrounded)
				{
					OnLandEvent.Invoke();

					//todo landParticles.Play();
					_animator.SetTrigger(IDLE_ANIMATOR_PARAMETER);
					_animator.SetBool(FALLING_ANIMATOR_PARAMETER, false);

					if (_mustSlam)
					{
						OnSlamEvent.Invoke();
						_mustSlam = false;
					}

				}

				_isGrounded = true;
			}
		}
		// #### Animations ######

		if (_isGrounded) // si esta tocando el piso:
		{
			// desactivar el salto si ya paso el tiempo de delay
			if (!_jumpTransitionDelayOn) 
			{
				_animator.SetBool(JUMP_ANIMATOR_PARAMETER, false);
			}
			//desctivar el doble salto
			_animator.SetBool(DOUBLE_JUMPING_ANIMATOR_PARAMETER, false);

			//activar el run si es que no está activada y está moviendose
			if (Mathf.Abs(m_Rigidbody2D.velocity.x)> 0.5f)
			{
				if (!_animator.GetBool(RUN_ANIMATOR_PARAMETER))
				{
					_animator.SetBool(RUN_ANIMATOR_PARAMETER, true);
				}
			}
			else
			{
				_animator.SetBool(RUN_ANIMATOR_PARAMETER, false);
			}
		}
		else
		{
			_animator.SetBool(RUN_ANIMATOR_PARAMETER, false);

			if (!_animator.GetBool(JUMP_ANIMATOR_PARAMETER))
			{
				_animator.SetBool(FALLING_ANIMATOR_PARAMETER, true);
			}
		}
	}

	#endregion

	#region IMovable Methods

	public void Move(float move, bool jump)
	{
        if (move > 0 && !_isFacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && _isFacingRight)
        {
            // ... flip the player.
            Flip();
        }

        if (!_player.CanMove())
		{
			m_Rigidbody2D.velocity = Vector2.zero;
			return;
		}

		//only control the player if grounded or airControl is turned on
		if (_isGrounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);

			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref _velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			
		}

		// If the player should jump...
		if (_isGrounded && jump)
		{
			// Add a vertical force to the player.
			_isGrounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			jump = false;
			_canDoubleJump = true;
			_animator.SetBool(JUMP_ANIMATOR_PARAMETER, true);

			_jumpTransitionDelayOn = true;
			StartCoroutine(JumpTransitionDelay());
		}
		
		if(_canDoubleJump && jump &&!_isGrounded)
		{
			_animator.SetBool(DOUBLE_JUMPING_ANIMATOR_PARAMETER, true);
			_animator.SetTrigger(DOUBLE_JUMP_ANIMATOR_PARAMETER);

			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x ,0); 
			_canDoubleJump = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	#endregion

	private IEnumerator JumpTransitionDelay()
	{
		yield return new WaitForSeconds(.1f);
		_jumpTransitionDelayOn = false;
	}

	public bool CheckGround()
	{
		return _isGrounded;
	}

	public void SetMustSlam(bool mustSlam)
	{
		_mustSlam = mustSlam;
	}
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		_isFacingRight = !_isFacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
