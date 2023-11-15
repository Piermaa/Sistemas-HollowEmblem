using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
   void Attack(Vector3 attackOrigin, Vector3 direction);
   
   public void InitializeEnemyAttack(GameObject owner);
}
