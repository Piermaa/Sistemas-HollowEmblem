using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    #region Class Properties

    #region Public Properties

    public Vector3 RespawnPos => _respawnPos;
    public Transform CameraTarget => _cameraTarget;
    
    public enum DirectionsToAttack
    {
        Front, Up, Down,
    }
  
    #endregion
    #region Serialized Properties

    [SerializeField] private string _moveAxis = "Horizontal";
    [SerializeField] private KeyCode _aimUp = KeyCode.W;
    [SerializeField] private KeyCode _aimDown = KeyCode.S;
    [SerializeField] private KeyCode _attack = KeyCode.Mouse0;
    [SerializeField] private KeyCode _aim = KeyCode.Mouse1;
    [SerializeField] private KeyCode _reload = KeyCode.R;
    [SerializeField] private KeyCode _openInventory = KeyCode.I;
    [SerializeField] private KeyCode _openMap = KeyCode.M;
    [SerializeField] private KeyCode _toggleUI = KeyCode.Tab;
    [SerializeField] private KeyCode _jump = KeyCode.Space;
    [SerializeField] private AudioSource _abilityAudioSource;
    [SerializeField] private List<PlayerAbility> _playerAbilities = new();
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private CinemachineVirtualCamera _vcam;
     
    #endregion

    private Vector3 _respawnPos;
    private PlayerAttack _playerAttack;
    private PlayerShoot _playerShoot;
    private DirectionsToAttack directionsToAttack;
    private float _horizontalMove;
    private float _immunityTime=.2f;
    private bool _mustJump;
    private IMovable _movable;
    private float _immunityTimer;
    private Camera _cam;

    #endregion

    #region Commands

    private ReloadCmd _reloadCmd;
    private OpenInventoryCmd _openInventoryCmd;
    private OpenMapCmd _openMapCmd;
    private ToggleUIDisplayCmd _toggleUIDisplayCmd;
    private GameManager _gameManager;
    
    #endregion
    
    #region Monobehaviour Callbacks

    protected override void Awake()
    {
        base.Awake();
        _movable = GetComponent<IMovable>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerShoot = GetComponent<PlayerShoot>();
        _cam = Camera.main;

        foreach (var abilty in _playerAbilities)
        {
            abilty.Init(gameObject, _abilityAudioSource);
        }

        var uiDisplayer = GetComponentInChildren<UIDisplayer>();
        
        _openInventoryCmd = new(uiDisplayer);
        _openMapCmd = new(uiDisplayer);
        _toggleUIDisplayCmd = new(uiDisplayer);
        print("playerShoot");
        _reloadCmd = new ReloadCmd(_playerShoot);
    }

    private void Start()
    {
        ActionsManager.RegisterAction(ItemConstants.USE_HEAL);
        ActionsManager.SubscribeToAction(ItemConstants.USE_HEAL, Heal);
        _gameManager = GameManager.Instance;
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
        _movable.Move(_horizontalMove * Time.fixedDeltaTime, _mustJump);
        _mustJump = false;
    }

    #endregion

    private void InputProcess()
    {
        if (Input.GetKey(_aim) && _movable.CheckGround())
        {
            _playerShoot.Aim(true, (int)directionsToAttack);
            _movable.CanMove = false;
        }

        if (Input.GetKeyUp(_aim))
        {
            _playerShoot.Aim(false, (int)directionsToAttack);
            _movable.CanMove = true;
        }

        if (Input.GetKeyDown(_attack) && _playerShoot.IsAiming)
        {
            _playerShoot.Attack((int)directionsToAttack);
        }

        if (Input.GetKeyDown(_attack) && !_playerShoot.IsAiming && _playerAttack.CanAttack())
            _playerAttack.Attack((int)directionsToAttack);
        
        if (Input.GetKeyDown(_reload)) _gameManager.AddEvent(_reloadCmd);
        
        if (Input.GetKeyDown(_openInventory))_gameManager.AddEvent(_openInventoryCmd);
        if (Input.GetKeyDown(_openMap))_gameManager.AddEvent(_openMapCmd);
        if (Input.GetKeyDown(_toggleUI))_gameManager.AddEvent(_toggleUIDisplayCmd);
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
        abilityToUnlock.Init(gameObject, _abilityAudioSource);
    }

    #region Actor Methods Overrides

    public override void TakeDamage(int damageTaken)
    {
        if (_immunityTimer <= 0)
        {
            base.TakeDamage(damageTaken);
            _immunityTimer = _immunityTime;

            UIManager.Instance.GetHealthUIManager.SetHealth(CurrentHealth, MaxHealth);
        }
    }

    #endregion

    public void Heal()
    {
        //todo heal sound
        if (_currentHealth < MaxHealth)
        {
            _currentHealth++;
            UIManager.Instance.GetHealthUIManager.SetHealth(CurrentHealth, MaxHealth);
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

    public void ResumePlayerMovement()
    {
        _movable.CanMove = true;
    }

    public void StopPlayerMovement()
    {
        _movable.CanMove = false;
    }

    public bool CanMove()
    {
        return !_playerShoot.IsAiming && !_playerShoot.IsReloading;
    }

    public override void Death()
    {
        transform.position = RespawnPos;
        _currentHealth = MaxHealth;
        _vcam.Follow = _cameraTarget;
        _cam.GetComponent<ChangeAmbientMusic>().SetAmbienceMusic();
        _cam.GetComponent<AudioSource>().Play();
    }

    public void SetRespawnPosition(Transform newRespawnPosition)
    {
        _respawnPos = newRespawnPosition.position;
    }
}
