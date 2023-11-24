using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieFall : MonoBehaviour
{
    [SerializeField] private Transform respawnPosition;
    private int _damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(_damage);
            collision.gameObject.transform.position = respawnPosition.position;
        }
    }
}
