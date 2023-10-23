using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerEnemy : Enemy
{
    #region Class Properties
    public bool CanAttack
    {
        get => _canAttack;
        set => _canAttack = value;
    }
    
    #region Serialized Properties
    [SerializeField] [Tooltip("0: Patrol, 1: Chase")] private EnemyMovement[] _enemyMovements;
    [SerializeField] private EnemyAttack _enemyAttack;
    [SerializeField] private Transform _attackOrigin;
    #endregion
    private const string ATTACK_ANIMATOR_PARAMETER = "Attack";
    private EnemyMovement _currentEnemyMovement;
    private bool _canMove = true;
    private bool _canAttack = false;
    #endregion

    #region MonoBehaviour Callbacks
    protected override void Awake()
    {
        base.Awake();

        foreach (var enemyMovement in _enemyMovements)
        {
            enemyMovement.InitEnemyMovement(gameObject, _enemyStats as MovableEnemyStats);
        }
        
        _enemyAttack.InitializeEnemyAttack(_attackOrigin, gameObject);
        SwitchEnemyMovement(0);
    }
    protected override void Update()
    {
        base.Update();
        if(_canMove)
            _currentEnemyMovement.Move();

        if (_attackCooldownTimer<=0 && _canAttack)
        {
            _animator.SetTrigger(ATTACK_ANIMATOR_PARAMETER);
            _attackCooldownTimer = _enemyStats.AttackCooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackOrigin.position, .5f);
    }

    #endregion

    #region Class Methods
    private void SwitchEnemyMovement(int index)
    {
        _currentEnemyMovement = _enemyMovements[index];
    }

    #region Exposed Methods

    public void BeginChase()
    {
        SwitchEnemyMovement(1);
    }

    public void StopChase()
    {
        SwitchEnemyMovement(0);
    }

    public void Attack()
    {
        _enemyAttack.Attack();
    }

    //#################################
    //### Called in animation event ###
    //#################################
    public void CanMove()
    {
        _canMove = true;
    }

    public void CantMove()
    {
        _canMove = false;
    }


    #endregion
    #endregion
}