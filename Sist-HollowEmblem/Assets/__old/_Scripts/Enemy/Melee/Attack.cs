using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Classes")]
    [SerializeField] Animator animator;
    [SerializeField] BasicIA ia;
    [SerializeField] AudioSource attackSound;

    [Header("Bool")]
    bool canAttack;
    public bool playerInRange;

    [Header("Float")]
    [SerializeField] float attackCooldown=5;
    [SerializeField] float attackTimer;
    [SerializeField] float attackTime;

    private void Start()
    {
        attackTimer = attackCooldown;
    }
    private void Update()
    {
        if (attackTimer < 0)
        {
            canAttack = true;
     
        }
        else 
        {
            attackTimer -= Time.deltaTime;
        }

        if(playerInRange&&canAttack)
        {
            MeleeAttack();
        }
    }

 
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;  
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    /// <summary>
    /// If the cooldown allows it, the enemy attacks using an animation that moves a collider, if player enters on it it takes damage
    /// </summary>
    void MeleeAttack()
    {
        if (canAttack)
        {
            ia.StopWalking(attackTime); //the enemy will stop walkinguntil the attackTime expires
            animator.SetTrigger("Attack");
            canAttack = false;
            attackTimer = attackCooldown;

            attackSound.Play();
        }
    }
}
