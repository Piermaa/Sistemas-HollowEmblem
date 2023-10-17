using UnityEngine;

public class EnemyFootPatrol : EnemyMovement
{
    public float MoveSpeed => _enemyStats.MovementSpeed;

    #region Class Properties
    #region Serialized Properties

    [SerializeField] private Transform[] _waypoints;
    #endregion
    private int _waypointIndex;
    private float _waitTimeTimer;
    private bool _turned;
    #endregion

    #region Class Methods

    
    protected virtual void SetNextWaypoint()
    {
        _waypointIndex = (_waypointIndex) == _waypoints.Length-1 ? _waypointIndex = 0 : _waypointIndex + 1;
        Stop(_enemyStats.WaitTime);
        Transform newWaypoint = _waypoints[_waypointIndex];
        _currentWaypoint = new Vector3(newWaypoint.position.x ,transform.position.y);
        _turned = false;
    }
    
    public void Stop(float time) 
    {
        _waitTimeTimer = time;
    }
    #endregion

    #region EnemyMovement Overrided Methods

    public override void Move()
    {
        base.Move();
        
        _waitTimeTimer -= Time.deltaTime;

        if (_waitTimeTimer >= 0 )
        {
            _currentMoveSpeed = 0;
        }
        else
        {
            _currentMoveSpeed = MoveSpeed;
            if (!_turned)
            {
                SetScale();
            }
        }
        

        if (transform.position == _currentWaypoint)
        {
            SetNextWaypoint();
        }
    }

    public override void InitEnemyMovement(GameObject enemyGameObject, MovableEnemyStats enemyStats)
    {        
        base.InitEnemyMovement(enemyGameObject, enemyStats);
        SetNextWaypoint();
        _currentMoveSpeed = MoveSpeed;
    }

    public override void SetScale()
    {
        base.SetScale();
        _turned = true;
    }

    #endregion
}
