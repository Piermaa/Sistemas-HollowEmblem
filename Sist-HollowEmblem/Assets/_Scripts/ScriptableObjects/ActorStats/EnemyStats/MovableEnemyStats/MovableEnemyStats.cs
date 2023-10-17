using System;
using UnityEngine;
[CreateAssetMenu(fileName = "MovableEnemyStats", menuName = "Stats/ActorStats/MovableEnemyStats", order = 0)]
public class MovableEnemyStats : EnemyStats
{
    [SerializeField] protected MovableEnemyStatValues _movableEnemyStats;
    public float WaitTime => _movableEnemyStats.WaitTime;
    public float ChaseSpeed => _movableEnemyStats.ChaseSpeed;
}

[Serializable]
public struct MovableEnemyStatValues
{
    public float WaitTime;
    public float ChaseSpeed;
}

