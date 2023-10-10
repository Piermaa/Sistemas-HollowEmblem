using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesDamage : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject respawnPosition;

    private void Start()
    {
        player = FindObjectOfType<CharacterController2D>().gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player.transform.position = respawnPosition.transform.position;
            collision.gameObject.TryGetComponent<HealthController>(out var health);
            health.TakeDamage(1);
        }
    }

}
