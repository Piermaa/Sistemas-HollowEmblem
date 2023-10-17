using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : Actor
{
    #region Class Properties

    protected Animator _animator;
    protected EnemyStats _enemyStats;
    protected float _attackCooldownTimer;

    #endregion

    #region Monobehaviour Callbacks

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _enemyStats = _actorStats as EnemyStats;
    }

    protected virtual void Update()
    {
        _attackCooldownTimer = _attackCooldownTimer > 0 ? _attackCooldownTimer - Time.deltaTime : 0;
    }

    #endregion
   
}

