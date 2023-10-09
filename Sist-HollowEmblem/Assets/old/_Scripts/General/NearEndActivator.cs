using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearEndActivator : MonoBehaviour
{
    private TeleManDialogue teleMan;

    private void Awake()
    {
        teleMan = FindObjectOfType<TeleManDialogue>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            teleMan.nearEnd = true;
        }
    }
}
