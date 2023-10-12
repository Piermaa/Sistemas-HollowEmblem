using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : Enemy
{
   protected override void Update()
   {
      base.Update();
      if (_attackCooldownTimer<=0)
      {
         _enemyAttack.Attack();
         _attackCooldownTimer = _enemyStats.AttackCooldown;
      }
   }
}
