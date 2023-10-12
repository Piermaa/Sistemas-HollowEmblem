using System;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyStats", menuName = "Stats/ActorStats/EnemyStats", order = 0)]
public class EnemyStats : ActorStats
{
    [SerializeField] protected EnemyStatValues _enemyStats;
    public float AttackCooldown => _enemyStats.AttackCooldown;
    public int AttackDamage => _enemyStats.AttackDamage;
}

[Serializable]
public struct EnemyStatValues
{
    public float AttackCooldown;
    public int AttackDamage;
}

