using Cinemachine.Utility;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootAttack", menuName = "Attacks/EnemyAttacks/ShootAttack", order = 0)]
public class ShootAttack : EnemyAttack
{
    #region Class Properties

    #region Serialized Properties

    [SerializeField] private GameObject _bulletPrefab;
    #endregion
    private BulletFactory _bulletFactory;
    #endregion
    
    #region EnemyAttack Overrides

    public override void InitializeEnemyAttack(GameObject owner)
    {
        _bulletFactory = new BulletFactory(_bulletPrefab.GetComponent<Bullet>());
    }

    public override void Attack(Vector3 spawnPos, Vector3 direction)
    {
       var b= _bulletFactory.CreateProduct();
       b.Reset(spawnPos);
       b.Shoot(direction);
    }

    #endregion
}
