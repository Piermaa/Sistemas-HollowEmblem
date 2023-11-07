using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BossPhase
{
    public int HealthUmbral;
    public int AttackCooldown;
}

public class BossEnemy : Enemy
{
    #region Serialized Variables

    [SerializeField] private List<BossPhase> _bossPhases;
    [SerializeField] private int _phase=-1;

    #endregion

    #region Class Properties

    private BossPhase _currentPhase=default;
    private const string PHASE_INTEGER = "Phase";
    private const string ATTACK_ANIMATOR_PARAMETER = "Attack";
    private const string ATTACK_TRIGGER = "Attack";
    private float _bossAttackCooldown;
    #endregion

    #region Unity Callbacks

    private void Start()
    {
        ChangePhase();
        _bossAttackCooldown = _enemyStats.AttackCooldown;
    }
    
    protected override void Update()
    {
        base.Update();

        if (_attackCooldownTimer<=0)
        {
            _animator.SetTrigger(ATTACK_ANIMATOR_PARAMETER);
            _attackCooldownTimer = _bossAttackCooldown;
        }
    }
    
    #endregion

    #region Class Methods

    private bool CheckPhaseChange()
    {
        return CurrentHealth < _currentPhase.HealthUmbral;
    }

    private void ChangePhase()
    {
        _phase++;
        _attackCooldownTimer = 0;
        _animator.ResetTrigger(ATTACK_TRIGGER);
        if (_phase>0)
        {
       //     _currentPhase.Effects.SetActive(false);
        }
        _animator.SetInteger(PHASE_INTEGER, _phase);
        _currentPhase = _bossPhases[_phase];
        _bossAttackCooldown = _bossPhases[_phase].AttackCooldown;
        //   _currentPhase.Effects.SetActive(true);

        //    _cmdEnemyAttack = new(_currentPhase.PhaseEnemyAttack,_enemyStatsValues,attackOrigin);
        //    _agentController.SetAgent(_currentPhase.PhaseAgent);
    }

    #endregion
    
    #region Actor Overrided Methods

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (CheckPhaseChange())
        {
            ChangePhase();
        }
    }

    #endregion
}
