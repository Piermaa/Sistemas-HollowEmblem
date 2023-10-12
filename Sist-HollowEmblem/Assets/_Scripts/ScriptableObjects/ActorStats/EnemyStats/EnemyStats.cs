using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : ActorStats
{
    [SerializeField] protected EnemyStatValues _enemyStats;
    public float AttackCooldown => _enemyStats.AttackCooldown;
    public int AttackDamage => _enemyStats.AttackDamage;
}

public struct EnemyStatValues
{
    public float AttackCooldown;
    public int AttackDamage;
}

