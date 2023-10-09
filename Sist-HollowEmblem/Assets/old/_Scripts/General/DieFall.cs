using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieFall : MonoBehaviour
{
    PlayerCombat playerCombat;
    [SerializeField] HealthController healthController;

    public GameObject respawnPosition;

    private void Awake()
    {
        playerCombat = FindObjectOfType<PlayerCombat>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            healthController.healthPoints--;
            playerCombat.transform.position = respawnPosition.transform.position;
        }
    }
}
