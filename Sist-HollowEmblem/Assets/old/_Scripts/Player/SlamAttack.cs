using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SlamAttack : MonoBehaviour
{
    [SerializeField]GameObject slamEffect;
    [SerializeField]CharacterController2D controller;
    [SerializeField]PlayerAbilities playerAbilities;
    [SerializeField] HealthManager healthManager;

    [SerializeField] Animator animator;

    float slamTime;
    public float slamCoolDown=3;

    public LayerMask enemyLayer;

    private void Start()
    {
       
    }
    public void Update()
    {
        slamTime -= Time.deltaTime;
    }
    IEnumerator SlamEffects()
    {
        slamEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        slamEffect.SetActive(false);
    }
    public void Slam() //this method is called in the OnSlamEvent in the controller 2D
    {
        if (slamTime < 0)
        {
            animator.SetBool("Falling", false);
            animator.SetTrigger("Slam");


            slamTime = slamCoolDown;

            healthManager.SetInmunity();

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,5,enemyLayer);

            foreach (Collider2D collided in colliders)
            {
                collided.TryGetComponent<HealthController>(out var health);
                
                    health.TakeDamage(2);
                
            }
        }

    }
  
}
