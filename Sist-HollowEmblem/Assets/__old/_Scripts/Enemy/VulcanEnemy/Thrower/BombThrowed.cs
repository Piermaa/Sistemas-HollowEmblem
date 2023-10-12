using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class BombThrowed : MonoBehaviour, IPooledObject
{
    ObjectPooler objectPooler;
    Rigidbody2D rb2D;
    public float y;
    public float x;
    public float z;
    public float force;
    public string explosion ="Explosion";
    private void Start()
    {
        objectPooler = FindObjectOfType<ObjectPooler>(); 
    }
    public void OnObjectSpawn()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if (transform.rotation.z == 0 && x > 0)
        {
            x = -Mathf.Abs(x);
            Vector3 theScale = transform.localScale;
            theScale.x = -1;
            transform.localScale = theScale;
        }
        if((transform.rotation.z != 0 && x < 0))
        {
            x = Mathf.Abs(x);
            Vector3 theScale = transform.localScale;
            theScale.x = 1;
            transform.localScale = theScale;
        }
        rb2D.AddForce(new Vector2(x,y)*force,ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.CompareTag("Enemy")|| collision.CompareTag("Bomb")))
        {
            objectPooler.SpawnFromPool(explosion, transform.position, transform.rotation);
            this.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
                collision.gameObject.TryGetComponent<HealthController>(out var target);
                target.TakeDamage(1);
        }
    }
}
