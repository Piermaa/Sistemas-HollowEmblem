using UnityEngine;

[CreateAssetMenu(fileName = "VulcanAttack", menuName = "Attacks/EnemyAttacks/VulcanAttack", order = 0)]
public class VulcanAttack : EnemyAttack
{
    #region Class Properties

    #region Serialized Properties

    [SerializeField] private Vector2 _shootDirection;
    [SerializeField] private float _shootForce;
    [SerializeField] private GameObject _vulcanBulletPrefab;
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
        var l= _vulcanBulletFactory.CreateProduct();
        l.Reset(_spawnPos.position);
        l.Shoot(new Vector2(-_shootDirection.x,_shootDirection.y), _shootForce);
        
        var r= _vulcanBulletFactory.CreateProduct();
        r.Reset(_spawnPos.position);
        r.Shoot(_shootDirection, _shootForce);
    }

    #endregion
}
