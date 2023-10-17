using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
[CreateAssetMenu(fileName = "CircleOverlapAttack", menuName = "Attacks/EnemyAttacks/CircleOverlapAttack", order = 1)]
public class CircleOverlapAttack : EnemyAttack
{
    #region Class Properties

    #region Serialized Properties

    [SerializeField] private int _damage;
    [SerializeField] private float _attackRadius;
    [FormerlySerializedAs("whatIsPlayer")] [SerializeField] private LayerMask _whatIsPlayer;

    #endregion

    private Transform _attackOrigin;

    #endregion

    
    #region EnemyAttack Overrides

    public override void InitializeEnemyAttack(Transform attackOrigin, GameObject owner)
    {
        _attackOrigin = attackOrigin;
    }

    public override void Attack()
    {
       Physics2D.OverlapCircle(_attackOrigin.position, _attackRadius, _whatIsPlayer)? // veo si encuentro al player
           .GetComponent<IDamageable>().TakeDamage(_damage); // le hago daño
    }
    #endregion
}
