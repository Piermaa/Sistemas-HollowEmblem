using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesText : MonoBehaviour
{
    [SerializeField] GameObject Text;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Text.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Text.SetActive(false);
            Destroy(gameObject);
        }
    }
}
