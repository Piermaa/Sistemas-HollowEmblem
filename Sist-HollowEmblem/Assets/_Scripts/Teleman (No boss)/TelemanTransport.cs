using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelemanTransport : MonoBehaviour
{
    [SerializeField] private GameObject _teleman;
    [SerializeField] private Transform _newPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _teleman.transform.position = _newPosition.position;
        }
    }
}
