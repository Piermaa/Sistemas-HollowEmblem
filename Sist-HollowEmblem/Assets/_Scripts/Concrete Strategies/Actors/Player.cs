using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    
    [SerializeField] private KeyCode _dash = KeyCode.LeftShift;
    [SerializeField] private KeyCode _jump = KeyCode.Space;

    private float _horizontalMove;
    private float _inmunityTime;
    private bool _mustJump;
    private Animator _animator;
    #endregion

    private IMovable _movable;
    private float _inmunityTimer;

    #endregion

    #region Monobehaviour Callbacks

    protected override void Awake()
    {
        base.Awake();
        _movable = GetComponent<IMovable>();
        _animator = GetComponent<Animator>();
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
        //Movement
        
        MovementInputs();
    }

    private void MovementInputs()
    {
        if (Input.GetKeyDown(_jump)) _mustJump = true;
        _horizontalMove = Input.GetAxisRaw(_moveAxis) * _actorStats.MovementSpeed;
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
