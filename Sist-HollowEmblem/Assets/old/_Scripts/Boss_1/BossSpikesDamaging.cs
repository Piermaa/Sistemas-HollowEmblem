using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpikesDamaging : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.TryGetComponent<HealthController>(out var health);
            health.TakeDamage(1);
        }
    }
}
