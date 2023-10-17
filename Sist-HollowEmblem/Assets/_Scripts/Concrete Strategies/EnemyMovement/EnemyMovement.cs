using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IEnemyMovement
{
    #region Class Properties

    protected MovableEnemyStats _enemyStats;
    protected Vector3 _currentWaypoint;
    protected float _currentMoveSpeed;

    #endregion


    #region IEnemyMovement Methods
    
    public virtual void Stop(float time)
    {
    
    }
    
    public virtual void SetScale()
    {
        Vector3 dir = _currentWaypoint - transform.position;
        Vector3 theScale = transform.localScale;

        theScale.x = dir.x < 0 ? -1 : 1;
        
        if (theScale==transform.localScale)
            return;
        
        transform.localScale = theScale;
    }

    public virtual void Move()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, _currentWaypoint, _currentMoveSpeed * Time.deltaTime);
    }

    public virtual void InitEnemyMovement(GameObject enemyGameObject, MovableEnemyStats movableEnemyStats)
    {
        _enemyStats = movableEnemyStats;
        _currentMoveSpeed = _enemyStats.MovementSpeed;
        SetScale();
    }

    #endregion
}
