using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : Enemy
{
   #region Class Properties

   #region Serialized Properties

   [SerializeField] private float _warmUpTime=0;
   [SerializeField] private VulcanAttack[] _vulcanAttacks;
   [SerializeField] private ParticleSystem[] _shootParticles;
   [SerializeField] private Transform _attackOrigin;

   #endregion
   
   private const string ATTACK_ANIMATOR_PARAMETER = "Attack";

   #endregion

   private VulcanAttackCmd[] _vulcanAttackCmd;

   #region Monobehaviour Callbacks
    
   protected override void Awake()
   {
      base.Awake();
      _attackCooldownTimer += _warmUpTime;
      
      _vulcanAttackCmd = new VulcanAttackCmd[_vulcanAttacks.Length];
      
      for (int i = 0; i < _vulcanAttacks.Length; i++)
      {
         _vulcanAttackCmd[i] = new(_vulcanAttacks[i],_attackOrigin,gameObject,_shootParticles[i]);
      }
   }
   
   protected override void Update()
   {
      base.Update();
      if (_attackCooldownTimer<=0 && !_isDead)
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
      GameManager.Instance.AddEvent(_vulcanAttackCmd[index]);
   }
   
   #endregion
}
