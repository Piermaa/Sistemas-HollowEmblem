using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanShootTriggers : MonoBehaviour
{
    [SerializeField] GameObject text;
    private PlayerCombat playerCombat;

    private void Awake()
    {
        playerCombat = FindObjectOfType<PlayerCombat>();    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerCombat.canShoot)
        {
            text.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerCombat.canShoot)
        {
            text.SetActive(false);
            Destroy(gameObject);
        }
    }
}
