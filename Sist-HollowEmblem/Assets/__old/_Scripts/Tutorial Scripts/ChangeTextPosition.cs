using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextPosition : MonoBehaviour
{
    [SerializeField] GameObject textPosition;


    void Update()
    {
        transform.position = textPosition.transform.position;
    }
}
