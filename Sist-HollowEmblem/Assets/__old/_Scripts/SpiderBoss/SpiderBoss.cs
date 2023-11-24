using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { SEARCHING, CHARGING, EMBISTING, RECOVERING }

public class SpiderBoss : BossEnemy
{
    BattleState state;

    [SerializeField] private GameObject embistingTrigger;

    [Header("Raycast")]
    private RaycastHit2D playerRc;
    private RaycastHit2D wallRc;

    [Header("GameObject")]
    [SerializeField] private GameObject leftSide;
    [SerializeField] private GameObject rightSide;
    [SerializeField] private GameObject waypointA;
    [SerializeField] private GameObject waypointB;
    [SerializeField] private GameObject invulnerableArea;
    [SerializeField] private GameObject abUnlocker;

    [Header("Bools")]
    private bool isRight;
    private bool goingRight;
    private bool canEmbist;
    private bool canRecover;
    private bool isInvulnerable;

    [Header("Transforms")]
    [SerializeField] private Transform seekPlayerStart;

    [Header("Floats")]
    private float distanceOfRay = 25f;
    private float distanceOfWallRay = 0.5f;
    private float speed = 7.5f;
    private float embistingSpeed = 10f;
    private float backSpeed = 0.1f;
    private float cooldown;
    [SerializeField] private float cooldownTimer = 0.2f;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask spikeLayer;

    [Header("Sounds")]
    [SerializeField] private AudioSource stepSound;
    [SerializeField] private AudioSource step2Sound;
    [SerializeField] private AudioSource crashSound;
    [SerializeField] private AudioSource crash2Sound;
    [SerializeField] private AudioSource growlSound;

    protected override void Awake()
    {
        base.Awake();
        _currentHealth = MaxHealth;
        _currentPhase = _bossPhases[0];
        gameObject.SetActive(false);
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

    protected override void Update()
    {
        UpdateCooldown();
        SetInvulnerability();
        BossStateExecution();
    }

    void Searching()
    {
        Debug.Log("SEARCHING");
        Vector3 theScale = transform.localScale;

        if ((playerRc = Physics2D.Raycast(seekPlayerStart.position, seekPlayerStart.TransformDirection(Vector2.right), distanceOfRay, playerLayer)) && cooldown <= 0)
        {
            state = BattleState.CHARGING;
        }

        else
        {
            _animator.SetBool("Walk", true);

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
        _animator.SetBool("Walk", false);

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

        if (wallRc = Physics2D.Raycast(seekPlayerStart.position, seekPlayerStart.TransformDirection(Vector2.left), distanceOfWallRay, spikeLayer))
        {
            _animator.SetBool("Walk", false);
            state = BattleState.RECOVERING;
        }

        else
        {
            _animator.SetBool("Walk", true);
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
        _animator.SetBool("Walk", false);

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
            invulnerableArea.SetActive(true);
        }

        else
        {
            invulnerableArea.SetActive(false);
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

    public override void Death()
    {
        base.Death();

        crashSound.Play();
    }

    public void StepSoundEffect()
    {
        stepSound.Play();
    }

    public void Step2SoundEffect()
    {
        step2Sound.Play();
    }

    public override void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            base.TakeDamage(damage);
        }
    }
}
