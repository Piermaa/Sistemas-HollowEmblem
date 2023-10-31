using UnityEngine;

public interface IBullet
{
    int Damage { get; }
    void Reset(Vector2 spawnPosition);
}

public interface IVulcanBullet : IBullet
{
    void Shoot(Vector2 direction, float force);
}

public interface IAirEnemyBullet : IBullet
{
    void Shoot(Vector2 direction);
}