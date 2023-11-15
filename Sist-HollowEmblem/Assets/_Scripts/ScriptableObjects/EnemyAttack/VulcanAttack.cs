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
    #endregion
    
    #region EnemyAttack Overrides

    public override void InitializeEnemyAttack(GameObject owner)
    {
        _vulcanBulletFactory = new VulcanBulletFactory(_vulcanBulletPrefab.GetComponent<VulcanBullet>());
    }

    public override void Attack(Vector3 spawnPos, Vector3 direction)
    {
        var l= _vulcanBulletFactory.CreateProduct();
        l.Reset(spawnPos);
        l.Shoot(new Vector2(-_shootDirection.x,_shootDirection.y), _shootForce);
        
        var r= _vulcanBulletFactory.CreateProduct();
        r.Reset(spawnPos);
        r.Shoot(_shootDirection, _shootForce);
    }

    #endregion
}
