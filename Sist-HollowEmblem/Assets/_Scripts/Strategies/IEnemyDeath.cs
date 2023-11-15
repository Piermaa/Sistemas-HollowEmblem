using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyDeath
{
    void InitializeEnemyDeath(Enemy enemy);
    void Death();
}
