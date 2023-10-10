using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private PlayerSounds sounds;
    private CharacterController2D controller;
    private PlayerMovement movement;
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerCombat playerCombat;
    private HealthManager healthManager;
    StateManager stateManager;
    public LayerMask enemyLayer;

    [Header("Bool")]
    public bool isParrying;
    public bool unlockAll;
    public bool slamUnlocked;
    public bool willDestroy;

    [Header("Floats")]
    public float slamForce;
    public float slamCD = 3;
    float slamTime;
    float slamCoolDown;
    float slamTimer;

    public Image slamUI;
    public GameObject slamUIGameObject;

    private void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
        stateManager = GetComponent<StateManager>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        playerCombat = GetComponent<PlayerCombat>();
        rb = GetComponent<Rigidbody2D>();

        if (unlockAll)
        {
            AutoUnlock();
        }
    }

    private void Update()
    {
        slamTimer = slamTimer<0?0: slamTimer-Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftControl) && slamUnlocked && slamTimer <= 0)
        {
            if (!controller.CheckGround())
            {
                animator.SetBool("Falling", true);
                rb.AddForce(Vector2.down * slamForce, ForceMode2D.Impulse);
                willDestroy = true;
                controller.MustSlam();
                slamTimer = slamCD;
            }
        }

        if (controller.CheckGround())
        {
            animator.SetBool("Falling", false);
            if (willDestroy)
            {
                animator.SetTrigger("Slam");
            }
        }
               
        UISlam();
    }

    IEnumerator Destroy(GameObject ground)
    {
        ground.TryGetComponent<Animator>(out Animator gAnimator);
        gAnimator.SetTrigger("Destroy");
        yield return null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.collider.CompareTag( "Ground") || collision.collider.CompareTag("DestructibleGround")) && willDestroy)
        {
            if (collision.collider.CompareTag("DestructibleGround"))
            {
                StartCoroutine(Destroy(collision.gameObject));
              
            }
            sounds.PlaySound(sounds.slam);
        }

         willDestroy = false;
       
    }

    void AutoUnlock()
    {
        TryGetComponent<PlayerCombat>(out var combat);
        combat.canShoot = true;
        stateManager.enabled = enabled;
        slamUnlocked = true;
        TryGetComponent<PlayerMovement>(out var movement);
        movement.dashUnlocked = true;
    }

    public void SlamImpact()
    {
        animator.SetBool("Falling", false);
        animator.SetTrigger("Slam");


        slamTime = slamCoolDown;

        healthManager.SetInmunity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5, enemyLayer);

        foreach (Collider2D collided in colliders)
        {
            collided.TryGetComponent<HealthController>(out var health);

            health.TakeDamage(2);

        }
    }

 

    public void AbilityUnlock(string unlockedAb)
    {
        switch (unlockedAb)
        {
            case "Slime":
                stateManager.enabled = enabled;
                break;
            case "Dash":
                TryGetComponent<PlayerMovement>(out var movement);
                movement.dashUnlocked = true;
                    break;
            case "Slam":
                slamUnlocked = true;
                break;

            case "Shoot":
                playerCombat.canShoot = true;
                break;
        }
    }

    void UISlam()
    {
        slamUI.fillAmount = slamTimer / slamCD;

        if (slamUnlocked)
        {
            slamUIGameObject.SetActive(true);
        }

        else
        {
            slamUIGameObject.SetActive(false);
        }
    }
}
