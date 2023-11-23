using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelemanTransport : MonoBehaviour
{
    [SerializeField] private Dialogue _dialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Aaa me estan llamandoo");
            _dialogue.SetNextDialogue();
            gameObject.SetActive(false);
        }
    }
}
