using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IPlayerAttack
{
    //float delayTime;
    //public LayerMask layer;
    //Animator animator;
    //public Vector3 direction;
    //Rigidbody2D rb;
    //public Rigidbody2D playerRigidBody;
    //float forceMultiplier = 7;

    [SerializeField] private float _speed;
    [SerializeField] private Transform[] _attackDirections;
    private PlayerMovementController _playerMovementController;
    private Rigidbody2D _rigidbody2d;
    [SerializeField] private Transform _attackStartPosition;
    [SerializeField] private float _damage;


    #region IPlayerAttack Properties

    public float Speed => _speed;

    public PlayerMovementController PlayerMovementController => _playerMovementController;

    public Rigidbody2D Rigidbody2d => _rigidbody2d;

    public Transform AttackStartPosition => _attackStartPosition;

    public float Damage => _damage;
    #endregion

    private void Awake()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Attack(int direction)
    {
        var projectile = ObjectPooler.Instance.SpawnFromPool("PlayerAttack", _attackDirections[direction].position, _attackDirections[direction].rotation, _rigidbody2d, transform.localScale * -1);
        projectile.GetComponent<Projectile>().Direction = direction;

    }
}
