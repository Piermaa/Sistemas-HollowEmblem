using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAbility", menuName = "Abilities/Slam", order = 1)]
public class Slam : PlayerAbility
{
    #region Class Properties

    #region Serialized Properties

    [SerializeField] private float _slamForce = 5;
    [SerializeField] private float _slamRadius = 5;
    [SerializeField] private float _slamTimer;
    [SerializeField] private LayerMask _whatIsEnemy;
    #endregion

    private float _slamTimeTimer = 0;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private PlayerMovementController _playerMovementController;
    private SlamUIManager _slamUIManager;
    #endregion

    public override void Init(GameObject playerGameObject, AudioSource audioSource)
    {
        base.Init(playerGameObject, audioSource);
        _rigidbody2D = _playerGameObject.GetComponent<Rigidbody2D>();
        _animator = _playerGameObject.GetComponent<Animator>();
        _playerMovementController = _playerGameObject.GetComponent<PlayerMovementController>();
        
        _playerMovementController.OnSlamEvent.AddListener(SlamImpact);
        _slamUIManager = UIManager.Instance.GetSlamUIManager;
        _slamUIManager.UnlockAbility();
    }

   public override bool CanBeUsed()
   {
       return base.CanBeUsed() && !_playerMovementController.CheckGround();
   }

    public override void Use()
    {
        if (CanBeUsed())
        {
            _coolDownTimer = _cooldown;
            _rigidbody2D.velocity = Vector2.zero;
            _animator.SetTrigger("Slam");
            _rigidbody2D.AddForce(Vector2.down * _slamForce, ForceMode2D.Impulse);
            //willDestroy = true;
            _slamTimeTimer = _slamTimer;
            _playerMovementController.SetMustSlam(true);
        }
    }

    public override void AbilityUpdate()
    {
        base.AbilityUpdate();

        _slamUIManager.UpdateCooldown(_coolDownTimer, _cooldown);
        _slamTimeTimer -= Time.deltaTime;

        if (_slamTimeTimer < 0)
        {
            _slamTimeTimer = 0;
        }

        if (_playerMovementController.MustSlam)
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y); // evita que el jugador se mueva en x al slamear
        }
    }

    public void SlamImpact()
    {
        _playerMovementController.SetMustSlam(false);
        AbilityAudioSource.PlayOneShot(_abilitySound);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_playerGameObject.transform.position,5,_whatIsEnemy);

        foreach (Collider2D collided in colliders)
        {
            if (collided.CompareTag("Enemy") && collided.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(2);
            }

            if (collided.CompareTag("DestructibleGround"))
            {
                collided.GetComponent<Animator>().SetTrigger("Destroy");
            }
        }
    }
}
