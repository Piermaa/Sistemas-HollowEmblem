using UnityEngine;
using System;
public class ChaseRangePlayerDetector : PlayerDetector
{
    #region Class Properties
    #region Serialized Properties

    [SerializeField] private PatrollerEnemy _patrollerEnemy;
    [SerializeField] private EnemyFootChase _enemyFootChase;
    [SerializeField] private float _looseDistance = 30;
    #endregion
    #endregion

    #region MonoBehaviour Callbacks

    private void Update()
    {
        if (_patrollerEnemy.IsChasing)
        {
            if (_playerTransform!=null)
            {
                if (Vector2.Distance(transform.position,_playerTransform.position)>_looseDistance)
                {
                    _patrollerEnemy.StopChase();
                }
            }
        }
    }

    #endregion
    
    
    #region PlayerDetectorOverrides

    public override void OnPlayerDetect()
    {
        _enemyFootChase.SetPlayerTransform(_playerTransform);
        _patrollerEnemy.BeginChase();
    }

    #endregion
}
