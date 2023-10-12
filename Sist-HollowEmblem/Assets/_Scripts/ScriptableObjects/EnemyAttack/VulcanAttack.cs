using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "VulcanAttack", menuName = "Attacks/EnemyAttacks/VulcanATtack", order = 0)]
public class VulcanAttack : EnemyAttack
{
    [SerializeField] private GameObject _bulletPrefab;

    private Vector3 _spawnPos;
    public void InitializeEnemyAttack(Vector3 spawnpos)
    {
        _spawnPos = spawnpos;
    }

    public override void Attack()
    {
        //todo usar pool como corresponde gracias
        Instantiate(_bulletPrefab, _spawnPos, Quaternion.Euler(new Vector3(0,0,0))).GetComponent<IPooledObject>().OnObjectSpawn();
        Instantiate(_bulletPrefab, _spawnPos, Quaternion.Euler(new Vector3(0,0,-1))).GetComponent<IPooledObject>().OnObjectSpawn();;
    }
}
