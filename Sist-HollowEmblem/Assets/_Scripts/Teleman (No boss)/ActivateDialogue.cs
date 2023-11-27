using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.AssetImporters;
using UnityEngine;

public class ActivateDialogue : MonoBehaviour
{
    [SerializeField] private GameObject _dialoguePanel;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            //_dialoguePanel.GetComponent<TextMeshProUGUI>().text = string.Empty;
        }
    }
}
