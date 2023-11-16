using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoABB
{
    // datos a almacenar, en este caso un entero
    public int info;

    public string key;
    // referencia los nodos izquiero y derecho
    public NodoABB hijoIzq = null;
    public NodoABB hijoDer = null;

    public virtual void Process()
    {

    }
}

