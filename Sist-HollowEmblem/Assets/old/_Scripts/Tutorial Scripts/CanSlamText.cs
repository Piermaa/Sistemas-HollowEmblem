using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSlamText : MonoBehaviour
{
    [SerializeField] GameObject text;
    private PlayerAbilities playerAbilities;

    private void Awake()
    {
        playerAbilities = FindObjectOfType<PlayerAbilities>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerAbilities.slamUnlocked)
        {
            text.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerAbilities.slamUnlocked)
        {
            text.SetActive(false);
        }
    }
}
