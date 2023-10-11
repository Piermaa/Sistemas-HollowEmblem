using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityUnlocker : MonoBehaviour
{
    [SerializeField] private PlayerAbility _abilityToUnlock;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().UnlockAbility(_abilityToUnlock);
            gameObject.SetActive(false);
        }
    }
}
