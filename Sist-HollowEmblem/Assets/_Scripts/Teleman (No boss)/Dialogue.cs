using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [System.Serializable]
    public struct Lines
    {
        [SerializeField] public Transform _newPosition;
        [SerializeField] public string[] _lines;
    }

    private GameObject _teleman;
    [SerializeField] private Lines[] _dialogues;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private float _textSpeed = 0.1f;
    private int _dialogueIndex;
    private int _linesIndex;

    private void Awake()
    {
        _teleman = GameObject.FindGameObjectWithTag("Teleman");
        gameObject.SetActive(false);
    }

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (textComponent.text == _dialogues[_dialogueIndex]._lines[_linesIndex])
            {
                NextLine();
            }

            else
            {
                StopAllCoroutines();
                textComponent.text = _dialogues[_dialogueIndex]._lines[_linesIndex];
            }
        }
    }

    public void StartDialogue()
    {
        _linesIndex = 0;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in _dialogues[_dialogueIndex]._lines[_linesIndex].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(_textSpeed);
        }
    }

    void NextLine()
    {
        if (_linesIndex < _dialogues[_dialogueIndex]._lines.Length - 1)
        {
            _linesIndex++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }

        else
        { 
            gameObject.SetActive(false);
        }
    }

    public void SetNextDialogue()
    {
        textComponent.text = string.Empty;
        _dialogueIndex++;
        _linesIndex = 0;
        _teleman.transform.position = _dialogues[_dialogueIndex]._newPosition.position;
        StartDialogue();
    }
}
