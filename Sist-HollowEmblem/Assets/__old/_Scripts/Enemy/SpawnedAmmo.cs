using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class SpawnedAmmo : MonoBehaviour, IPooledObject
{
    Animator a;

    public void OnObjectSpawn()
    {
        a.SetTrigger("Spawned");
    }
}
