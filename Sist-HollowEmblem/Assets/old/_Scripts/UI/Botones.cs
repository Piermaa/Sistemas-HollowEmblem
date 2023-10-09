using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Botones : MonoBehaviour
{
    ControlTextos controlTextos;
    public int indice;
    private void Start()
    {
        controlTextos = FindObjectOfType<ControlTextos>();

        GetComponent<Button>().onClick.AddListener(controlTextos.ElegirTexto);

        //GetComponent<Button>().onClick.
    }
}
