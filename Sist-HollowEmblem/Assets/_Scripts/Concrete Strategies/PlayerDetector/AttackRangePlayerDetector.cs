using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangePlayerDetector : PlayerDetector
{
    [SerializeField] private PatrollerEnemy _patrollerEnemy;

    #region PlayerDetector Overrided Methods

    public override void OnPlayerDetect()
    {
        _patrollerEnemy.CanAttack = true;
    }

    public override void OnPlayerLoose()
    {
        _patrollerEnemy.CanAttack = false;
    }

    #endregion
   
}
