using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    #region Class Properties

    #region Public Properties

    public enum DirectionsToAttack
    {
        Front, Up, Down,
    }
    private DirectionsToAttack directionsToAttack;
    #endregion
    #region Serialized Properties

    [SerializeField] private string _moveAxis = "Horizontal";
    [SerializeField] private KeyCode _aimUp = KeyCode.W;
    [SerializeField] private KeyCode _aimDown = KeyCode.S;
    [SerializeField] private KeyCode _attack = KeyCode.Mouse0;
    [SerializeField] private KeyCode _aim = KeyCode.Mouse1;
    [SerializeField] private KeyCode _reload = KeyCode.R;
    
    [SerializeField] private KeyCode _jump = KeyCode.Space;
    [SerializeField] private AudioSource _abilityAudioSource;

    [SerializeField] private List<PlayerAbility> _playerAbilities = new();
    private PlayerAttack _playerAttack;
    private PlayerShoot _playerShoot;

    private float _horizontalMove;
    private float _immunityTime;
    private bool _mustJump;
    #endregion

    private IMovable _movable;
    private float _immunityTimer;

    #endregion

    #region Monobehaviour Callbacks
    protected override void Awake()
    {
        base.Awake();
        _movable = GetComponent<IMovable>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerShoot = GetComponent<PlayerShoot>();

        foreach (var abilty in _playerAbilities)
        {
           abilty.Init(gameObject,_abilityAudioSource);
        }
    }

    private void Start()
    {
        ActionsManager.RegisterAction(ItemConstants.USE_HEAL);
        ActionsManager.SubscribeToAction(ItemConstants.USE_HEAL, Heal);
    }

    private void Update()
    {
        InputProcess();
        AbilityInputsAndCooldowns();
        SetAttackDirection();
    }

    private void FixedUpdate()
    {
        _immunityTimer = _immunityTimer > 0 ? _immunityTimer - Time.deltaTime : 0;
        _movable.Move(_horizontalMove* Time.fixedDeltaTime, _mustJump);
        _mustJump = false;
    }

    #endregion

    private void InputProcess()
    {
        // if (Input.GetKeyDown(_aim)) Aiming();
        if (Input.GetKey(_aim) && _movable.CheckGround())
        {
            _playerShoot.Aim(true);
            _movable.CanMove = false;
        }
        else
        {
            _playerShoot.Aim(false);
            _movable.CanMove = true;
        }

        if (Input.GetKeyDown(_attack) && _playerShoot.IsAiming)
        {
            _playerShoot.Attack((int)directionsToAttack);
        }

        if (Input.GetKeyDown(_attack) && !_playerShoot.IsAiming)
        {
            _playerAttack.Attack((int)directionsToAttack);
        }
        
        if (Input.GetKeyDown(_reload))
        {
            _playerShoot.Reload();
        }

        //  if (Input.GetKeyUp(_aim)) StopAiming();

        //=======================debug========================
        if (Input.GetKeyDown(KeyCode.T)) TakeDamage(1);
 
        MovementInputs();
    }

    private void MovementInputs()
    {
        if (Input.GetKeyDown(_jump)) _mustJump = true;
        _horizontalMove = Input.GetAxisRaw(_moveAxis) * _actorStats.MovementSpeed;
    }

    
    /// <summary>
    /// Updates ability cooldowns and checks if their required input in order to use them 
    /// </summary>
    private void AbilityInputsAndCooldowns()
    {
        foreach (var ability in _playerAbilities)
        {
            ability.AbilityUpdate();
            
            if (Input.GetKeyDown(ability.AbilityKey))
            {
                ability.Use();
            }
        }
    }

    public void UnlockAbility(PlayerAbility abilityToUnlock)
    {
        _playerAbilities.Add(abilityToUnlock);
        abilityToUnlock.Init(gameObject,_abilityAudioSource);
    }


    #region Actor Methods Overrides

    public override void TakeDamage(int damageTaken)
    {
        if (_immunityTimer<=0)
        {
            base.TakeDamage(damageTaken);
            _immunityTimer = _immunityTime;
        }
    }

    #endregion

    public void Heal()
    {
        //todo heal sound
        if (_currentHealth<MaxHealth)
        {
            _currentHealth++;
        }
    }

    public void SetAttackDirection()
    {
        float y = Input.GetAxis("Vertical");

        if (y == 0)
        {
            directionsToAttack = DirectionsToAttack.Front;
        }

        if (y > 0)
        {
            directionsToAttack = DirectionsToAttack.Up;
        }

        if (y < 0)
        {
            directionsToAttack = DirectionsToAttack.Down;
        }
    }
}
