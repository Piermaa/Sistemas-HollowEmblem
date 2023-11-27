using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAttack
{
    void Attack(int direction);
    Transform AttackStartPosition { get; }
    float Damage { get; }
}
