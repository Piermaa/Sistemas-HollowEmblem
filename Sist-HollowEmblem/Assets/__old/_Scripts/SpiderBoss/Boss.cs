using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { SEARCHING, CHARGING, EMBISTING, RECOVERING }

public class Boss : MonoBehaviour
{
    BattleState state;

    public Animator animator;

    public GameObject embistingTrigger;

    public BoxCollider2D damageCollider;

    [Header("Raycast")]
    RaycastHit2D playerRc;
    RaycastHit2D wallRc;

    [Header("GameObject")]
    public GameObject leftSide;
    public GameObject rightSide;
    public GameObject waypointA;
    public GameObject waypointB;
    public GameObject invulnerabilityShield;
    public GameObject abUnlocker;

    [Header("Bools")]
    public bool drop;
    public bool isRight;
    public bool goingRight;
    public bool canEmbist;
    public bool canRecover;
    public bool isInvulnerable;

    [Header("Transforms")]
    public Transform seekPlayerStart;
    public Transform playerTransform;

    [Header("Floats")]
    private float distanceOfRay = 25f;
    public float distanceOfWallRay = 0.5f;
    public float speed = 7.5f;
    public float embistingSpeed = 10f;
    public float backSpeed = 0.1f;
    public float cooldown;
    [SerializeField] private float cooldownTimer = 0.2f;

    [Header("LayerMasks")]
    public LayerMask playerLayer;
    public LayerMask spikeLayer;

    [Header("Sounds")]
    [SerializeField] AudioSource stepSound;
    [SerializeField] AudioSource step2Sound;
    [SerializeField] AudioSource crashSound;
    [SerializeField] AudioSource crash2Sound;
    [SerializeField] AudioSource growlSound;
    //[SerializeField] AudioSource dieSound;

    private void Awake()
    {
        gameObject.SetActive(false);
        damageCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cooldown = cooldownTimer;
        canEmbist = true;
        canRecover = true;
        isInvulnerable = true;
        gameObject.SetActive(true);
        embistingTrigger.SetActive(false);
        goingRight = true;
        state = BattleState.SEARCHING;
    }

    private void Update()
    {
        UpdateCooldown();
        SetInvulnerability();
        BossStateExecution();
    }

    void Searching()
    {
        Debug.Log("SEARCHING");
        Vector3 theScale = transform.localScale;

        if ((playerRc = Physics2D.Raycast(seekPlayerStart.position, seekPlayerStart.TransformDirection(Vector2.left), distanceOfRay, playerLayer)) && cooldown <= 0)
        {   
                state = BattleState.CHARGING;
        }

        else
        {
            animator.SetBool("Walk", true);

            if (goingRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, waypointA.transform.position, speed * Time.deltaTime);
            }

            else
            {
                transform.position = Vector2.MoveTowards(transform.position, waypointB.transform.position, speed * Time.deltaTime);
            }

            if (transform.position.x == waypointA.transform.position.x)
            {
                goingRight = false;
                theScale.x *= -1;
                transform.localScale = theScale;
            }

            else if (transform.position.x == waypointB.transform.position.x)
            {
                goingRight = true;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }
    }

    IEnumerator Charging()
    {
        Debug.Log("CHARGING");

        canEmbist = false;
        animator.SetBool("Walk", false);

        for (float i = 1.5f; i > 0; i -= Time.deltaTime)
        {

            if (goingRight)
            {
                transform.rotation = Quaternion.Euler(0, 0, 10);
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 0.005f, transform.position.y), backSpeed * Time.deltaTime);
            }

            else
            {
                transform.rotation = Quaternion.Euler(0, 0, -10);
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 0.005f, transform.position.y), backSpeed * Time.deltaTime);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.5f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        canEmbist = true;
        growlSound.Play();
        state = BattleState.EMBISTING;

        yield return null;
    }

    void Embisting()
    {
        Debug.Log("EMBISTING");
        
        isInvulnerable = false;
        damageCollider.enabled = true;

        if (wallRc = Physics2D.Raycast(seekPlayerStart.position, seekPlayerStart.TransformDirection(Vector2.left), distanceOfWallRay, spikeLayer))
        {
            animator.SetBool("Walk", false);
            state = BattleState.RECOVERING;
        }

        else
        {
            animator.SetBool("Walk", true);
            embistingTrigger.SetActive(true);

            if (goingRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, rightSide.transform.position, embistingSpeed * Time.deltaTime);
            }

            else
            {
                transform.position = Vector2.MoveTowards(transform.position, leftSide.transform.position, embistingSpeed * Time.deltaTime);
            }
        }
    }

    IEnumerator Recovering()
    {
        Debug.Log("RECOVERING");

        canRecover = false;
        embistingTrigger.SetActive(false);
        animator.SetBool("Walk", false);
        
        if (goingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 10);
        }
        
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -10);
        }

        yield return new WaitForSeconds(3f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        cooldown = cooldownTimer;
        canRecover = true;
        isInvulnerable = true;
        damageCollider.enabled = false;
        state = BattleState.SEARCHING;

        yield return null;
    }

    void UpdateCooldown()
    {
        cooldown -= Time.deltaTime;

        if (cooldown <= 0)
            cooldown = 0;
    }

    void SetInvulnerability()
    {
        if (isInvulnerable)
        {
            damageCollider.enabled = false;
            invulnerabilityShield.SetActive(true);
        }

        else
        {
            damageCollider.enabled = true;  
            invulnerabilityShield.SetActive(false);
        }
    }

    void BossStateExecution()
    {
        switch (state)
        {
            case BattleState.SEARCHING:
                Searching();
                break;

            case BattleState.CHARGING:

                if (canEmbist)
                {
                    StartCoroutine(Charging());
                }

                break;

            case BattleState.EMBISTING:
                Embisting();
                break;

            case BattleState.RECOVERING:

                if (canRecover)
                {
                    crashSound.Play();
                    crash2Sound.Play();
                    StartCoroutine(Recovering());
                }
        
                break;
        }
    }

    public void Death()
    {
        crashSound.Play();
        if (drop)
        {
            //GameManager.Instance.StartVictory(this.transform.position, "Dash");
        }
    }
    public void StepSoundEffect()
    {
        stepSound.Play();
    }
    public void Step2SoundEffect()
    {
        step2Sound.Play();
    }
}
