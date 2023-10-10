using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour, IPooledObject
{
    Rigidbody2D rb;
    public float speed;
    public float timeToDespawn;
    public float timeLimit = 5;
    public float xvelocity;
    public Vector3 moveDirection;

    bool verifyScale=false;
  

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {

      
    }

    public void OnObjectSpawn()
    {
        rb.velocity = Vector3.zero;
        verifyScale = true;
        moveDirection -= transform.position;
        moveDirection.Normalize();
        //Debug.Log("VelocimoveDirection * speed,ty before force:" +rb.velocity);
        rb.velocity = moveDirection * speed;
      
        //Debug.Log("Velocity after force:" + rb.velocity);
        if (verifyScale)
        {
            if (rb.velocity.x < 0)
            {
                var partscale = GetComponentInChildren<ParticleSystem>().gameObject.transform;
                Vector3 theScale = transform.localScale;
                theScale.x = 1;
                this.transform.localScale = theScale;
                theScale = partscale.localScale;
                theScale.x = 1;
                partscale.localScale = theScale;
            }
            else
            {
                var partscale = GetComponentInChildren<ParticleSystem>().gameObject.transform;
                Vector3 theScale = transform.localScale;
                theScale.x = -1;
                this.transform.localScale = theScale;
                theScale = partscale.localScale;
                theScale.x = -1;
                partscale.localScale = theScale;

            }
            verifyScale = false;
        }
    }
    private void Update()
    {
      

        xvelocity = rb.velocity.x;
        timeToDespawn += Time.deltaTime;

        if (timeToDespawn>timeLimit)
        {
            //var pool =  FindObjectOfType<ObjectPooler>();
           this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<HealthController>(out var health))
            {
                health.TakeDamage(1);
            }
        }
        this.gameObject.SetActive(false);
    }


    //-----------------------Feliz Cumple Dante-----------------------|
    //                          $10000                                |
    //                                                                |
    //                                                                |
    //                                                                |
    //                     navidad                                    |
    //                                                                |
    //                                                                |
    //                                                                |
    //----------------------------------------------------------------|
}
