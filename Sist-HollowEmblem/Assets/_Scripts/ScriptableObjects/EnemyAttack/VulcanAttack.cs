using UnityEngine;

[CreateAssetMenu(fileName = "VulcanAttack", menuName = "Attacks/EnemyAttacks/VulcanATtack", order = 0)]
public class VulcanAttack : EnemyAttack
{
    #region Class Properties

    #region Serialized Properties

    [SerializeField] private GameObject _leftBulletPrefab;
    [SerializeField] private GameObject _rightBulletPrefab;

    #endregion
    private VulcanBulletFactory _leftVulcanBulletFactory;
    private VulcanBulletFactory _rightVulcanBulletFactory;
    private Vector3 _spawnPos;
    #endregion
    
    #region EnemyAttack Overrides

    public override void InitializeEnemyAttack(Transform attackOrigin, GameObject owner)
    {
        _leftVulcanBulletFactory = new VulcanBulletFactory(_leftBulletPrefab.GetComponent<VulcanBullet>());
        _rightVulcanBulletFactory = new VulcanBulletFactory(_rightBulletPrefab.GetComponent<VulcanBullet>());
        _spawnPos = attackOrigin.position;
    }

    public override void Attack()
    {
        _leftVulcanBulletFactory.CreateProduct().transform.position = _spawnPos;
        _rightVulcanBulletFactory.CreateProduct().transform.position = _spawnPos;
    }

    #endregion
}
