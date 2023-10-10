using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTextos : MonoBehaviour
{
    public int parrila;
    // Start is called before the first frame update
    public void ElegirTexto()
    {
        SexoSinConsensuacion(parrila);
    }
    public void SexoSinConsensuacion(int a)
    {
        print(a);
    }
}
