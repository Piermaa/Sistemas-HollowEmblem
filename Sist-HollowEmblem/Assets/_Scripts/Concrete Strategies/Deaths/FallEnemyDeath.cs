using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEnemyDeath : MonoBehaviour, IEnemyDeath
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void InitializeEnemyDeath(Enemy enemy)
    {
     //   throw new System.NotImplementedException();
    }

    public void Death()
    {
        GetComponent<PatrollerEnemy>().Death();
        _animator.SetTrigger("Death");
        gameObject.layer = 0;
  
        _rigidbody2D.gravityScale = 1;
        _rigidbody2D.AddForce(Vector2.down*3, ForceMode2D.Impulse);
    }
}
