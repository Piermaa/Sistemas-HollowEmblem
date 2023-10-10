using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D),(typeof(Rigidbody2D)))]
public class Actor : MonoBehaviour, IDamageable
{
    #region IDamageable Properties

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _actorStats.MaxLife;
    public SpriteRenderer ActorSprite => _spriteRenderer;
    public Material FlashingWhiteMaterial => _flashingWhiteMaterial;
    public ParticleSystem TakingDamageParticles { get; }
    public AudioSource TakingDamageSound { get; }

    #endregion

    #region Class Properties
    #region Serialized Properties

    [SerializeField] protected ActorStats _actorStats;
    [SerializeField] private Material _flashingWhiteMaterial;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ParticleSystem _takingDamageParticles;
    [SerializeField] private AudioSource _takingDamageSound;

    #endregion
    
    private int _currentHealth;
    private Material _baseMaterial;
    
    #endregion
    
    #region Monobehaviour Callbacks
    protected virtual void Awake()
    {
        _currentHealth = MaxHealth;
        _baseMaterial = _spriteRenderer.material;
    }
    #endregion

    #region IDamageable

    public IEnumerator TakingDamageFlash()
    {
        _spriteRenderer.material = _flashingWhiteMaterial;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.material = _baseMaterial;
    }

    public virtual void TakeDamage(int damageTaken)
    {
        _takingDamageSound.PlayOneShot(_takingDamageSound.clip);
        if (_currentHealth>0)
        {   
            _currentHealth -= damageTaken;
            _takingDamageParticles.Play();
            StartCoroutine(TakingDamageFlash());

            if (_currentHealth<=0)
            {
                Death();
            }
        }
    }

    public virtual void Death()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
