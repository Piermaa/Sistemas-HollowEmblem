using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextPosition : MonoBehaviour
{
    [SerializeField] private GameObject textPosition;

    void Start()
    {
        transform.position = textPosition.transform.position;
    }
}
