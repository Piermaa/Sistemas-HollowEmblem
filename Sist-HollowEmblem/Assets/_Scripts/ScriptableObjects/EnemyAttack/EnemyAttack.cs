using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAttack : ScriptableObject, IEnemyAttack
{
    
    public virtual void Attack(Vector3 attackOrigin, Vector3 direction)
    {
        
    }

    public virtual void InitializeEnemyAttack(GameObject owner)
    {
        
    }

}
