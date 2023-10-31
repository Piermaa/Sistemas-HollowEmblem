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
    private Transform _spawnPos;
    private Transform _enemy;
    #endregion
    
    #region EnemyAttack Overrides

    public override void InitializeEnemyAttack(Transform attackOrigin, GameObject owner)
    {
        _bulletFactory = new BulletFactory(_bulletPrefab.GetComponent<Bullet>());
        _spawnPos = attackOrigin;
        _enemy = owner.transform;
    }

    public override void Attack()
    {
       var b= _bulletFactory.CreateProduct();
       b.Reset(_spawnPos.position);
       b.Shoot(_enemy.localScale);
    }

    #endregion
}
