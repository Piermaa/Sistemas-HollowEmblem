using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlocker : MonoBehaviour
{
    [Tooltip("Dash (includes dj), Slam, Slime, Shoot")]
    public string unlockedAb;

    PlayerSounds playerSounds;

    private void Awake()
    {
        playerSounds = FindObjectOfType<PlayerSounds>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.TryGetComponent<PlayerAbilities>(out var abilities);
            abilities.AbilityUnlock(unlockedAb);
            playerSounds.PlaySound(playerSounds.pickupable);

            gameObject.SetActive(false);
        }
    }
}
