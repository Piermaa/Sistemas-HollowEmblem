using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour, IPlayerAttack
{
    [SerializeField] private Transform _attackStartPosition;
    [SerializeField] private float _damage;
    [SerializeField] private bool _isAiming;
    [SerializeField] private int _bulletsRemaining; // Es SerializeField para verlo en el Inspector, después lo borro
    private int _maxBullets = 10;
    private Animator _animator;
    private PlayerInventory _playerInventory;

    #region IPlayerAttack properties
    public PlayerMovementController PlayerMovementController => throw new System.NotImplementedException();

    public GameObject Projectile => throw new System.NotImplementedException();

    public Rigidbody2D Rigidbody2d => throw new System.NotImplementedException();

    public Transform AttackStartPosition => _attackStartPosition;

    public float Speed => throw new System.NotImplementedException();

    public float Damage => _damage;
    #endregion

    public bool IsAiming => _isAiming;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerInventory = GetComponent<PlayerInventory>();
    }

    public void Attack(int direction)
    {
        if (Physics2D.Raycast(_attackStartPosition.transform.position, _attackStartPosition.transform.forward, 100))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(_attackStartPosition.transform.position, _attackStartPosition.transform.forward, 100);

            if (hit2D.transform.CompareTag("Enemy") && hit2D.transform.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(1);
            }
        }
    }

    public void Aim (bool isTrue)
    {
        _isAiming = isTrue;
        _animator.SetTrigger("AimFront");
        _animator.SetBool("Aiming", _isAiming);
    }

    public void Reload()
    {
        _bulletsRemaining += _playerInventory.BulletsInInventory;

        if (_bulletsRemaining >= _maxBullets)
        {
            _bulletsRemaining = _maxBullets;
        }
    }
}
