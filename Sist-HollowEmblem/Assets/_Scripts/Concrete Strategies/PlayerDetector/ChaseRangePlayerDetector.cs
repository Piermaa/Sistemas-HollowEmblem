using UnityEngine;
using System;
public class ChaseRangePlayerDetector : PlayerDetector
{
    #region Class Properties
    #region Serialized Properties

    [SerializeField] private PatrollerEnemy _patrollerEnemy;
    [SerializeField] private EnemyFootChase _enemyFootChase;

    #endregion
    #endregion

    #region PlayerDetectorOverrides

    public override void OnPlayerDetect()
    {
        _enemyFootChase.SetPlayerTransform(_playerTransform);
        _patrollerEnemy.BeginChase();
    }

    public override void OnPlayerLoose()
    {
        _patrollerEnemy.StopChase();
    }

    #endregion
  
}
