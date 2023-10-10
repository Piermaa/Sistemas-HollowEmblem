using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int CurrentHealth { get; }
    int MaxHealth { get; }
    SpriteRenderer ActorSprite { get; } // el sprite que flashea a blanco
    Material FlashingWhiteMaterial { get; }
    ParticleSystem TakingDamageParticles { get; }
    AudioSource TakingDamageSound { get; }
    IEnumerator TakingDamageFlash();
    void TakeDamage(int damageTaken);
    void Death();
}
