using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAttack
{
    void Attack();
    void SetAttackDirection();
    PlayerMovementController PlayerMovementController { get; }
    GameObject Projectile { get; }
    Rigidbody2D Rigidbody2d { get; }
    Transform AttackStartPosition { get; }
    float Speed { get; }
    float Damage { get; }
}
