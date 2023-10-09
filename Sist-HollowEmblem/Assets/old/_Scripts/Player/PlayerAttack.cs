using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IPooledObject
{
   
    float delayTime;
    public LayerMask layer;
    Animator animator;
    public Vector3 direction;
    Rigidbody2D rb;
    public Rigidbody2D playerRigidBody;
    float forceMultiplier=7;
    public void OnObjectSpawn()
    {
        delayTime = 0.15f;
        if (direction.y == 0)
        {
            rb.AddForce(new Vector2(-transform.localScale.x,0) * forceMultiplier, ForceMode2D.Impulse); 
        }
        else { 
        rb.AddForce(direction*forceMultiplier, ForceMode2D.Impulse);
        }
        animator.SetTrigger("Spawn");
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        delayTime = delayTime>0 ? delayTime-Time.deltaTime : 0;
        if (delayTime<=0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.TryGetComponent<HealthController>(out var enemyH);
            enemyH.TakeDamage(1);

            playerRigidBody.velocity = Vector2.zero;
            playerRigidBody.AddForce((playerRigidBody.gameObject.transform.position - this.gameObject.transform.position).normalized*5, ForceMode2D.Impulse);
         
        }
    }
}
