using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerBullet : IBullet
{
    void Attack(Vector3 direction);
}
