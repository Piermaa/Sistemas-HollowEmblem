using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedEnemyDeath : MonoBehaviour, IEnemyDeath
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void InitializeEnemyDeath(Enemy enemy)
    {
       
    }

    public void OnDeathFinish()
    {
        Destroy(gameObject);
    }

    public void Death()
    {
        _animator.ResetTrigger("Attack");
        _animator.ResetTrigger("Shoot");
        _animator.SetTrigger("Death");
    }

    public void Revive()
    {
        _animator.SetTrigger("Revive");
    }
}
