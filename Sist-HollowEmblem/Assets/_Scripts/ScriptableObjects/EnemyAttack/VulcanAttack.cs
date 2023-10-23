using UnityEngine;

[CreateAssetMenu(fileName = "VulcanAttack", menuName = "Attacks/EnemyAttacks/VulcanAttack", order = 0)]
public class VulcanAttack : EnemyAttack
{
    #region Class Properties

    #region Serialized Properties

    [SerializeField] private GameObject _vulcanBulletPrefab;
    [SerializeField] private VulcanBulletStats _vulcanBulletStats;
    #endregion
    private VulcanBulletFactory _vulcanBulletFactory;
    private Transform _spawnPos;
    #endregion
    
    #region EnemyAttack Overrides

    public override void InitializeEnemyAttack(Transform attackOrigin, GameObject owner)
    {
        _vulcanBulletFactory = new VulcanBulletFactory(_vulcanBulletPrefab.GetComponent<VulcanBullet>());
        _spawnPos = attackOrigin;
    }

    public override void Attack()
    {
        _vulcanBulletFactory.CreateProduct(_spawnPos.position,_spawnPos.rotation,1,_vulcanBulletStats);
        _vulcanBulletFactory.CreateProduct(_spawnPos.position,_spawnPos.rotation,-1,_vulcanBulletStats);
    }

    #endregion
}