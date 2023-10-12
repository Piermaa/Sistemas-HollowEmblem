using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : Enemy
{
   [SerializeField] private Transform _attackOrigin;
   [SerializeField] private VulcanAttack[] _vulcanAttacks;
   private const string ATTACK_ANIMATOR_PARAMETER = "Shoot";

   protected override void Awake()
   {
      base.Awake();
      foreach (var vulcanAttack in _vulcanAttacks)
      {
         vulcanAttack.InitializeEnemyAttack(_attackOrigin.position);
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
/// <summary>
/// LLamar en animation event
/// </summary>
/// <param name="index"></param>
   public void Attack(int index)
   {
      _vulcanAttacks[index].Attack();
   }
}
