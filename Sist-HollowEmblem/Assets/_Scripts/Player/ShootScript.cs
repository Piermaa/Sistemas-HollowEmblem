using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public int damage = 10;

    [SerializeField] Animator animator;
    [SerializeField] GameObject fpsCam;
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
                target.TakeDamage(damage);
                //Instantiate(hitWeakpoint, hit2D.transform.position, hit2D.transform.rotation);

            }
        }
    }
}
