using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathCmd : IMemento
{
    public bool CanUndo => _canUndo;
    public float TimeToUndo { get=>_timeToUndo; set=> _timeToUndo=value; }
    private float _timeToUndo;
    private bool _canUndo;
    private IEnemyDeath _enemyDeath;
    private Enemy _enemy;
    public EnemyDeathCmd(IEnemyDeath enemyDeath, Enemy enemy)
    {
        _enemyDeath = enemyDeath;
        _enemy = enemy;
        if (enemy.CanRevive)
        {
            _canUndo = true;
            _timeToUndo = enemy.ReviveTime;
        }
    }

    public void Do()
    {
       _enemyDeath.Death();
    }

    public void Undo()
    {
        _canUndo = false;
        (_enemyDeath as AnimatedEnemyDeath).Revive();
        _enemy.CurrentHealth=(int)(_enemy.MaxHealth *.5f);
    }
}
