using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledObject
{
    //para cambiar el start por un void que se ejecuta al spawnear el objeto
    void OnObjectSpawn();
}
