using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    #region Serialized Variables

    [SerializeField] private List<BossPhase> _bossPhases;
    [SerializeField] private int _phase=0;

    #endregion

    #region Class Properties

    private BossPhase _currentPhase=default;
    private const string PHASE_INTEGER = "Phase";

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        ChangePhase(_phase);
    }

    #endregion

    #region Class Methods

    private bool CheckPhaseChange()
    {
        return CurrentHealth < _currentPhase.HealthUmbral;
    }

    private void ChangePhase(int index)
    {
        _attackTimer = 0;
        _animator.ResetTrigger(ATTACK_TRIGGER);
        if (index>0)
        {
            _currentPhase.Effects.SetActive(false);
        }
        _animator.SetInteger(PHASE_INTEGER,_phase);
        _currentPhase = _bossPhases[index];
        _currentPhase.Effects.SetActive(true);
        
        _cmdEnemyAttack = new(_currentPhase.PhaseEnemyAttack,_enemyStatsValues,attackOrigin);
        _agentController.SetAgent(_currentPhase.PhaseAgent);
    }

    #endregion
    
    #region Actor Overrided Methods

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (CheckPhaseChange())
        {
            _phase++;
            ChangePhase(_phase);
        }
    }

    #endregion
}
