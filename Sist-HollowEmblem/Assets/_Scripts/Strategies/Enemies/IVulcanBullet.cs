using UnityEngine;

public interface IVulcanBullet
{
    Vector2 ImpulseDirection { get; }
    float Force{ get; }
    string ExplosionObjectPoolerKey { get; }
}
