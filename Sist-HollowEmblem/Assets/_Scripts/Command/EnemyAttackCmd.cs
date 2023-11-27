using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCmd : ICommand
{
    private EnemyAttack _enemyAttack;
    private Transform _attackOrigin;
    private GameObject _owner;
    public EnemyAttackCmd(EnemyAttack newEnemyAttack, Transform attackOrigin, GameObject owner)
    {
        _enemyAttack = newEnemyAttack;
        _attackOrigin = attackOrigin;
        _owner = owner;
        _enemyAttack.InitializeEnemyAttack(owner);
    }

    public void Do()
    {
        _enemyAttack.Attack(_attackOrigin.position,-_owner.transform.localScale);
    }
}
