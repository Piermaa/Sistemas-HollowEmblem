using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour, IPlayerAttack
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject fpsCam;
    [SerializeField] float _damage;

    public PlayerMovementController PlayerMovementController => throw new System.NotImplementedException();

    public GameObject Projectile => throw new System.NotImplementedException();

    public Rigidbody2D Rigidbody2d => throw new System.NotImplementedException();

    public Transform AttackStartPosition => throw new System.NotImplementedException();

    public float Speed => throw new System.NotImplementedException();

    public float Damage => _damage;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShotSniper();
        }
    }

    public void ShotSniper()
    {
        animator.SetTrigger("Shot");
        if (Physics2D.Raycast(fpsCam.transform.position, fpsCam.transform.forward, 100))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(fpsCam.transform.position, fpsCam.transform.forward, 100);

            Debug.Log(hit2D.collider.name);
            Target target = hit2D.transform.GetComponent<Target>();

            //Instantiate(hitOther, hit.transform.position, hit.transform.rotation);

            if (target != null)
            {
                target.TakeDamage(Damage);
                //Instantiate(hitWeakpoint, hit2D.transform.position, hit2D.transform.rotation);
            }
        }
    }

    public void Attack(int direction)
    {
        throw new System.NotImplementedException();
    }

    public void SetAttackDirection()
    {
        throw new System.NotImplementedException();
    }
}
