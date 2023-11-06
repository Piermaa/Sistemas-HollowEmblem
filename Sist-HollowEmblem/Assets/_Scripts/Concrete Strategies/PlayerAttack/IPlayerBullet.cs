using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerBullet
{
    void Reset(Transform attackDirectionTransform);
    void Attack(Vector3 direction);
}
