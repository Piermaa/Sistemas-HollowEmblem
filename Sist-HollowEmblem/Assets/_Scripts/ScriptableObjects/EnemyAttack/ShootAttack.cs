using UnityEngine;

[CreateAssetMenu(fileName = "ShootAttack", menuName = "Attacks/EnemyAttacks/ShootAttack", order = 0)]
public class ShootAttack : EnemyAttack
{
    #region Class Properties

    #region Serialized Properties

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private BulletStats _bulletStats;
    #endregion

    private BulletFactory _bulletFactory;
    private Transform _spawnPos;
    #endregion
    
    #region EnemyAttack Overrides

    public override void InitializeEnemyAttack(Transform attackOrigin, GameObject owner)
    {
        _bulletFactory = new BulletFactory(_bulletPrefab.GetComponent<Bullet>());
        _spawnPos = attackOrigin;
    }

    public override void Attack()
    {
        _bulletFactory.CreateProduct(_spawnPos.position, Quaternion.identity, 
             (int)_spawnPos.parent.localScale.x, _bulletStats);
    }

    #endregion
}
