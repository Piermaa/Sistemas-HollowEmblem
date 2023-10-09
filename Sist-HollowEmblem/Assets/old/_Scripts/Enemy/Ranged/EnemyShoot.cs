using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Lean.Pool;
public class EnemyShoot : MonoBehaviour
{
    ObjectPooler objectPooler;
    public string ammunition;
    public float fireRate=10;
    float fireCooldown;

    public Transform spawnPos;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        fireCooldown = fireRate;
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if(fireCooldown<0 )
        {
            fireCooldown = fireRate;
            objectPooler.SpawnFromPool(ammunition, spawnPos.position, spawnPos.rotation);
        }

    }
}
