using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private GameObject _projectile;    // Se instancia desde la pool I guess
    [SerializeField] private float _speed;
    [SerializeField] private Transform[] _attackDirections;
    private PlayerMovementController _playerMovementController;
    private Rigidbody2D _rigidbody2d;
    [SerializeField] private Transform _attackStartPosition;
    [SerializeField] private float _damage;


    #region IPlayerAttack Properties
    public GameObject Projectile => _projectile;

    public float Speed => _speed;

    public PlayerMovementController PlayerMovementController => _playerMovementController;

    public Rigidbody2D Rigidbody2d => _rigidbody2d;

    public Transform AttackStartPosition => _attackStartPosition;

    public float Damage => _damage;
    #endregion

    public enum DirectionsToAttack
    {
        Up, Down, Front
    }
    public DirectionsToAttack directionsToAttack;

    private void Awake()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Attack(int direction)
    {
        ObjectPooler.Instance.SpawnFromPool("PlayerAttack", _attackDirections[direction].position, _attackDirections[direction].rotation, _rigidbody2d, transform.localScale);
    }

    public void SetAttackDirection()
    {
        float y = Input.GetAxis("Vertical");

        if (y == 0)
        {
            directionsToAttack = DirectionsToAttack.Front;

            _attackStartPosition.position = _attackDirections[0].position;
            _attackStartPosition.rotation = _attackDirections[0].rotation;

        }

        if (y > 0)
        {
            directionsToAttack = DirectionsToAttack.Up;

            _attackStartPosition.position = _attackDirections[1].position;
            _attackStartPosition.rotation = _attackDirections[1].rotation;

        }

        if (y < 0)
        {
            directionsToAttack = DirectionsToAttack.Down;
            _attackStartPosition.position = _attackDirections[2].position;
            _attackStartPosition.rotation = _attackDirections[2].rotation;

        }
    }
}