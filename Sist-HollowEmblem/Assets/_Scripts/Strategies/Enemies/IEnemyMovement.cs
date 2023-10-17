using UnityEngine;

public interface IEnemyMovement
{
    void Stop(float time);
    void Move();
    void InitEnemyMovement(GameObject enemyGameObject, MovableEnemyStats movableEnemyStats);
    void SetScale();
}
