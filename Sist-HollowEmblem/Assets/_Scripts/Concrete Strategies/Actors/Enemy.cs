using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : Actor
{
    protected IEnemyAttack _enemyAttack;
    protected float _attackCooldownTimer;

    protected EnemyStats _enemyStats;

    protected override void Awake()
    {
        base.Awake();
        _enemyStats = _actorStats as EnemyStats;
    }

    protected virtual void Update()
    {
        _attackCooldownTimer = _attackCooldownTimer > 0 ? _attackCooldownTimer - Time.deltaTime : 0;
    }
}

