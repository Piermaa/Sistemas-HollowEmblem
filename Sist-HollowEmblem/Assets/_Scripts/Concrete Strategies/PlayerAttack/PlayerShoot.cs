using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour, IPlayerAttack
{
    //[SerializeField] Animator animator;
    [SerializeField] Transform _attackStartPosition;
    [SerializeField] float _damage;
    [SerializeField] private bool _isAiming;

    #region IPlayerAttack properties
    public PlayerMovementController PlayerMovementController => throw new System.NotImplementedException();

    public GameObject Projectile => throw new System.NotImplementedException();

    public Rigidbody2D Rigidbody2d => throw new System.NotImplementedException();

    public Transform AttackStartPosition => _attackStartPosition;

    public float Speed => throw new System.NotImplementedException();

    public float Damage => _damage;
    #endregion

    public bool IsAiming => _isAiming;

    private void Update()
    {
        
    }

    public void Attack(int direction)
    {
        //animator.SetTrigger("Shot");}
        Debug.Log("IS SHOOTINNN");
        if (Physics2D.Raycast(_attackStartPosition.transform.position, _attackStartPosition.transform.forward, 100))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(_attackStartPosition.transform.position, _attackStartPosition.transform.forward, 100);

            if (hit2D.transform.CompareTag("Enemy") && hit2D.transform.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(1);
            }
        }
    }

    public void Aim(bool isTrue)
    {
        _isAiming = isTrue;
    }
}
