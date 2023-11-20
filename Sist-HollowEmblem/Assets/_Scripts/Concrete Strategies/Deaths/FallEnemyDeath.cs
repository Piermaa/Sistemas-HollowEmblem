using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class FallEnemyDeath : MonoBehaviour, IEnemyDeath
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private Enemy _enemy;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _enemy = GetComponent<Enemy>();
    }

    public void InitializeEnemyDeath(Enemy enemy)
    {
     //   throw new System.NotImplementedException();
    }

    public void Death()
    {
        _enemy.SetDead();
        _animator.SetTrigger("Death");
        gameObject.layer = 0;
  
        _rigidbody2D.gravityScale = 1;
        _rigidbody2D.AddForce(Vector2.down*3, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player") && _enemy.IsDead)
        {
            _enemy.DropItem();
            gameObject.SetActive(false);
        }
    }
}
