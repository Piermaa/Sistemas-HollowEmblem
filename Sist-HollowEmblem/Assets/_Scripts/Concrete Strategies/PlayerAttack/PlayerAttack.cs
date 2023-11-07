using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAttack : MonoBehaviour, IPlayerAttack
{
    //float delayTime;
    //public LayerMask layer;
    //Animator animator;
    //public Vector3 direction;
    //Rigidbody2D rb;
    //public Rigidbody2D playerRigidBody;
    //float forceMultiplier = 7;
    [FormerlySerializedAs("attackSound")] [SerializeField] private AudioSource _attackSound;
    [SerializeField] private float _speed;
    [SerializeField] private Transform[] _attackDirections;
    private PlayerMovementController _playerMovementController;
    private Rigidbody2D _rigidbody2d;
    [SerializeField] private Transform _attackStartPosition;
    [SerializeField] private float _damage;
    private float _timerToAttack;
    private float _cooldownToAttack = 0.3f;
    private int _currentDirection = 0;
    private Animator _animator;

    #region IPlayerAttack Properties

    public float Speed => _speed;

    public PlayerMovementController PlayerMovementController => _playerMovementController;

    public Rigidbody2D Rigidbody2d => _rigidbody2d;

    public Transform AttackStartPosition => _attackStartPosition;

    public float Damage => _damage;
    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMovementController = GetComponent<PlayerMovementController>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _timerToAttack = _cooldownToAttack;
    }

    private void Update()
    {
        _timerToAttack -= Time.deltaTime;

        if (_timerToAttack <= 0)
        {
            _timerToAttack = 0;
        }
    }
    
    public void OnAttack()
    {
        _timerToAttack = _cooldownToAttack;
        var projectile = ObjectPooler.Instance.SpawnFromPool("PlayerAttack");
        
        var playerBullet = projectile.GetComponent<IPlayerBullet>();
        playerBullet.Reset(_attackDirections[_currentDirection], _rigidbody2d);

        Vector3 attackDirection =Vector3.zero; 

        if (_currentDirection!=0)
        {
            attackDirection.y = _currentDirection == 1 ? 1 : -1;
        }
        else
        {
            attackDirection.x = -transform.localScale.x;
        }

        playerBullet.Attack(attackDirection);
    }

    public void Attack(int direction)
    {
        if (_timerToAttack!=0)
        {
            return;
        }

        _timerToAttack = _cooldownToAttack; 
        
        _currentDirection = direction;

        _animator.SetBool("Jump", false);
        
        switch (direction)
        {
            case 0:
                _attackSound.Play();
                _animator.SetTrigger("AttackFront");
                break;
            case 2:

                if (!_playerMovementController.CheckGround())
                {
                    _attackSound.Play();
                    _animator.SetTrigger("AttackDown");
                }

                break;
            case 1:
                _attackSound.Play();
                _animator.SetTrigger("AttackUp");
                break;
        }
    }

    public bool CanAttack()
    {
        return _timerToAttack <= 0;
    }
}
