using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    #region Class Properties
    #region Serialized Properties

    [SerializeField] private string _moveAxis = "Horizontal";
    [SerializeField] private KeyCode _aimUp = KeyCode.W;
    [SerializeField] private KeyCode _aimDown = KeyCode.S;
    [SerializeField] private KeyCode _attack = KeyCode.Mouse0;
    [SerializeField] private KeyCode _aim = KeyCode.Mouse1;
    [SerializeField] private KeyCode _reload = KeyCode.R;
    
    [SerializeField] private KeyCode _jump = KeyCode.Space;

    [SerializeField] private List<PlayerAbility> _playerAbilities = new();
    [SerializeField] private AudioSource _abilityAudioSource;
    private float _horizontalMove;
    private float _inmunityTime;
    private bool _mustJump;
    #endregion

    private IMovable _movable;
    private float _inmunityTimer;

    #endregion

    #region Monobehaviour Callbacks

    protected override void Awake()
    {
        base.Awake();
        _movable = GetComponent<IMovable>();

        foreach (var abilty in _playerAbilities)
        {
           abilty.Init(gameObject,_abilityAudioSource);
        }
    }

    private void Update()
    {
        InputProcess();
    }

    private void FixedUpdate()
    {
        _inmunityTimer = _inmunityTimer > 0 ? _inmunityTimer - Time.deltaTime : 0;
        _movable.Move(_horizontalMove* Time.fixedDeltaTime, _mustJump);
        _mustJump = false;
    }

    #endregion

    private void InputProcess()
    {
       // if (Input.GetKeyDown(_aim)) Aiming();
        
       // if (Input.GetKeyDown(_attack)) //GameManager.instance.AddEvents(_cmdAttack);
       // if (Input.GetKeyDown(_reload)) //GameManager.instance.AddEvents(_cmdReload);

      //  if (Input.GetKeyUp(_aim)) StopAiming();
        
        //=======================debug========================
        if (Input.GetKeyDown(KeyCode.T)) TakeDamage(1);
 
        
        MovementInputs();
        AbilityInputsAndCooldowns();
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
        if (_inmunityTimer<=0)
        {
            base.TakeDamage(damageTaken);
            _inmunityTimer = _inmunityTime;
        }
    }

    #endregion
}
