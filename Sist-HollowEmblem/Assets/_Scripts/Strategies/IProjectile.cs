using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    float DelayTime { get; }
    float DelayCooldown { get; }
    float Speed { get; }
    float ForceMultiplier { get; }
    LayerMask Layer { get; }
    int Direction { get; set; }
    Rigidbody2D Rb { get; }
}
