using System;
using System.Collections;


using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : Actor
{
    #region Class Properties
    
    public float ReviveTime => _reviveTime;
    public bool CanRevive  => _canRevive;

    public bool IsDead => _isDead;

    [SerializeField] protected bool _canRevive;
    [SerializeField] protected float _reviveTime=5;
    [SerializeField] protected bool _isDead = false;
    protected Animator _animator;
    protected EnemyStats _enemyStats;
    protected float _attackCooldownTimer;

    #endregion

    private EnemyDeathCmd _enemyDeathCmd;

    #region Monobehaviour Callbacks

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _enemyStats = _actorStats as EnemyStats;
        _enemyDeathCmd = new(GetComponent<IEnemyDeath>(),this);
    }

    protected virtual void Update()
    {
        _attackCooldownTimer = _attackCooldownTimer > 0 ? _attackCooldownTimer - Time.deltaTime : 0;
    }

    public override void TakeDamage(int damageTaken)
    {
        if (_currentHealth>0)
        {
            base.TakeDamage(damageTaken);
        }
    }

    public override void Death()
    {
       GameManager.Instance.AddEvent(_enemyDeathCmd);
    }
    
    public void SetDead()
    {
        _isDead = true;
    }

    public void SetAlive()
    {
        _isDead = false;
    }

    public virtual void DropItem()
    {
        GameManager.Instance.GetItemGameManager.SpawnItem()
            .gameObject.transform.position= transform.position;
    }

    #endregion
   
}

