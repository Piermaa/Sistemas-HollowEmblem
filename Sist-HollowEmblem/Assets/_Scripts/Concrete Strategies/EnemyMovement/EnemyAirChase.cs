using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAirChase : EnemyFootChase
{
    [SerializeField] private Vector2 _chasingOffset=new Vector2(5,1.5f);

    #region Monobehaviour Callbacks
    private void OnDrawGizmos()
    {
        Gizmos.color=Color.cyan;
        Gizmos.DrawSphere(_currentWaypoint,.1f);
    }
    
    #endregion
    
    #region EnemyMovement Overrided Methods

    public override void Move()
    {
        float dirMultpiplier = (_playerTransform.position.x < transform.position.x) ? 1 : -1;

        _currentWaypoint = _playerTransform.position + new Vector3(_chasingOffset.x*dirMultpiplier, _chasingOffset.y);
        
        if ( Vector2.Distance(transform.position,_currentWaypoint) > _stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, _currentWaypoint,4*Time.deltaTime);
        }
        
        SetScale();
    }

    public override void SetScale()
    {
        if (!_playerTransform)
        {
            return;
        }
        Vector3 dir = _playerTransform.position - transform.position;
        Vector3 theScale = transform.localScale;

        theScale.x = dir.x > 0 ? -1 : 1;
        
        if (theScale==transform.localScale)
            return;
        
        transform.localScale = theScale;
    }

    #endregion
}
