using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulcanAttackCmd : ICommand
{
    private EnemyAttack _enemyAttack;
    private Transform _attackOrigin;
    private GameObject _owner;
    private ParticleSystem _particleSystem;
    public VulcanAttackCmd(EnemyAttack newEnemyAttack, Transform attackOrigin, GameObject owner, ParticleSystem particleSystem)
    {
        _enemyAttack = newEnemyAttack;
        _attackOrigin = attackOrigin;
        _owner = owner;
        _particleSystem = particleSystem;
        _enemyAttack.InitializeEnemyAttack(owner);
    }

    public void Do()
    {
        _enemyAttack.Attack(_attackOrigin.position,_owner.transform.localScale);
        _particleSystem.Play();
    }
}
