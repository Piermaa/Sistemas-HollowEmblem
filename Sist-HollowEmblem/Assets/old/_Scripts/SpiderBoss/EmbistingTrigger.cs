using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbistingTrigger : MonoBehaviour
{
    public int damage = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.TryGetComponent<HealthController>(out var playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
