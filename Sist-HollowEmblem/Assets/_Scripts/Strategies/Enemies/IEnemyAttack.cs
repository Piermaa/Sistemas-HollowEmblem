using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
   void Attack();
   
   public void InitializeEnemyAttack(Transform attackOrigin, GameObject owner);
}
