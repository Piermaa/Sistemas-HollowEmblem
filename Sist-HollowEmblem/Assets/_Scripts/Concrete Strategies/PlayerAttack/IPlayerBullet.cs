using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerBullet
{
    void Reset(Transform attackDirectionTransform, Rigidbody2D rb);
    void Attack(Vector3 direction);
}
