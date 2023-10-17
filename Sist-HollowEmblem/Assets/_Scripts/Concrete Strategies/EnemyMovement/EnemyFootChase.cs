using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootChase : EnemyMovement
{
   #region Class Properties

   [SerializeField] protected float _stopDistance;
   protected Transform _playerTransform;

   #endregion

   #region Class Methods
   public void SetPlayerTransform(Transform playerTransform)
   {
      if (_playerTransform != null)
         return;

      _currentMoveSpeed = _enemyStats.ChaseSpeed;
      _playerTransform = playerTransform;
   }
   

   #endregion

   #region EnemyMovement Overrided Methods

   public override void Move()
   {
      SetScale();

      _currentWaypoint = new Vector3(_playerTransform.position.x, transform.position.y);

      if (Vector2.Distance(transform.position, _currentWaypoint) > _stopDistance)
      {
         base.Move();
      }
   }

   #endregion
}
