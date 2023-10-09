using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deactivator : MonoBehaviour
{
    [SerializeField] GameObject tutorial;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            tutorial.SetActive(false);
        }
    }
}
