using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootUnlocker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerShoot>().UnlockShoot();
        }
    }
}
