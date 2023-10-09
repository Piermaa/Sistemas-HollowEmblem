using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float healthPoints =30;
    // Start is called before the first frame update
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        
    }
    private void Update()
    {
        if (healthPoints < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        if (this.TryGetComponent<HealthController>(out var playerHealth))
        {
            playerHealth.TakeDamage(1);
        }
        else
        {
            healthPoints -= damage;
        }
    }
}
