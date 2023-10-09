using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    Vector3 respawnPosition;
    [SerializeField]PlayerSounds sounds;
    HealthController health;
    [SerializeField] HealthController[] bossHealth;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Sprite playerSprite;
    int progress;
    private void Start()
    {
        respawnPosition = transform.position;
        health = GetComponent<HealthController>();
    }

    private void Update()
    {
        
    }
    public void Respawn()
    {
        health.FullHeal();

        foreach (HealthController health in bossHealth)
        {
            health.FullHeal();
        }

        transform.position = respawnPosition;

        sprite.color = Color.white;

        TryGetComponent<PlayerCombat>(out var combat);
        combat.canAttack = true;
        TryGetComponent<Animator>(out var animator);
        animator.SetTrigger("Idle");
        animator.SetBool("Jump", false);
        animator.SetBool("Run", false);
        animator.SetBool("DoubleJumping", false);
        animator.SetBool("Falling", false);

        sprite.sprite = playerSprite;
        sounds.PlaySound(sounds.die);
    }
    public void SetRespawn(Vector3 newPos)
    {
        respawnPosition = newPos;       
    }
}
