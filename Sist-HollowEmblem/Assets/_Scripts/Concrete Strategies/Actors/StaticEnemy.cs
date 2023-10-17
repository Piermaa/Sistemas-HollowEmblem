using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : Enemy
{
   #region Class Properties

   #region Serialized Properties

   [SerializeField] private VulcanAttack[] _vulcanAttacks;
   [SerializeField] private ParticleSystem[] _shootParticles;
   [SerializeField] private Transform _attackOrigin;

   #endregion
   
   private const string ATTACK_ANIMATOR_PARAMETER = "Shoot";

   #endregion

   #region Monobehaviour Callbacks
    
   protected override void Awake()
   {
      base.Awake();
      foreach (var vulcanAttack in _vulcanAttacks)
      {
         vulcanAttack.InitializeEnemyAttack(_attackOrigin, gameObject);
      }
   }
   
   protected override void Update()
   {
      base.Update();
      if (_attackCooldownTimer<=0)
      {
         _animator.SetTrigger(ATTACK_ANIMATOR_PARAMETER);
         _attackCooldownTimer = _enemyStats.AttackCooldown;
      }
   }
   #endregion

   #region Class Methods
    
   /// <summary>
   /// LLamar en animation event
   /// </summary>
   /// <param name="index"></param>
   public void Attack(int index)
   {
      _vulcanAttacks[index].Attack();
      _shootParticles[index].Play();
   }

   #endregion
}
