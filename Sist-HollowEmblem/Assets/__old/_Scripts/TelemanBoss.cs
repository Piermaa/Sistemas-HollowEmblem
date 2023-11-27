using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FBBattleState { SPAWNING, IDLE, MELEEATTACK, FLOORATTACK, SHOOTATTACK, DEATH};

public class TelemanBoss : BossEnemy
{
    public FBBattleState state;

    private Rigidbody2D rb;
    private ObjectPooler objectPooler;
    private RaycastHit2D hit;
    public LayerMask playerLayer;
    private SceneChanger changer;

    [SerializeField] private Animator[] _floorSpots;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private EnemyAttack _shootAttack;
    private int attackIndex;

    [SerializeField] private bool canSpawning;
    [SerializeField] private bool canIdle;
    [SerializeField] private bool canMelee;
    [SerializeField] private bool canFly;
    [SerializeField] private bool canFloor;
    [SerializeField] private bool canDie;
    [SerializeField] private bool hasFloorAttacked;
    [SerializeField] private bool canShoot;
    [SerializeField] private bool isRight;
    [SerializeField] private bool canChangeScale;

    [Header("Float")] private float distanceOfRay = -4f;
    private float speed = 5f;
    private float flySpeed = 5.5f;

    [Header("Transform")] [SerializeField] private Transform raycastStart;
    [SerializeField] private Transform hangingSpot;
    [SerializeField] private Transform floorSpot;
    [SerializeField] private Transform shootStartUp;
    [SerializeField] private Transform shootStartDown;
    [SerializeField] private Transform shootStartMiddle;

    [Header("Vector3")] private Vector3 direction;
    private Vector3 directionDown;

    [Header("Sounds")] [SerializeField] private AudioSource meleeAttackSound;
    [SerializeField] private AudioSource shootAttackSound;
    [SerializeField] private AudioSource floorAttackSound;
    [SerializeField] private AudioSource dieSound;

    protected override void Awake()
    {
        base.Awake();
    
        _shootAttack.InitializeEnemyAttack(gameObject);
        _currentPhase = _bossPhases[0];
        gameObject.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        objectPooler = ObjectPooler.Instance;
        changer = FindObjectOfType<SceneChanger>();
    }

    void Start()
    {
        canSpawning = true;
        state = FBBattleState.SPAWNING;
    }

    protected override void Update()
    {
        BossStateExecution();
        ChangeScale();
    }

    IEnumerator SpawningAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        canIdle = true;
        state = FBBattleState.IDLE;
        yield return null;
    }

    IEnumerator Vulnerable()
    {
        canMelee = true;

        yield return new WaitForSeconds(1.2f);

        attackIndex = Random.Range(0, 3);
        SetAttackState();
        canIdle = true;

        yield return null;

    }

    void MeleeAttackActivator()
    {
        if (hit = Physics2D.Raycast(raycastStart.position, raycastStart.TransformDirection(Vector2.left), distanceOfRay,
                playerLayer))
        {
            canChangeScale = false;
            _animator.SetBool("Walking", false);
            _animator.SetTrigger("MeleeAttack");
        }

        else
        {
            Vector2 playerVector = new Vector2(_playerTransform.position.x, this.transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, playerVector, speed * Time.deltaTime);
            _animator.SetBool("Walking", true);
        }
    }

    void FloorAttackActivator()
    {
        if (transform.position == hangingSpot.transform.position)
        {
            if (!hasFloorAttacked)
            {
                _animator.SetBool("Flying", false);
                _animator.SetTrigger("FloorAttack");

                foreach (Animator anim in _floorSpots)
                {
                    anim.SetTrigger("Attack");
                }

                hasFloorAttacked = true;
            }
        }

        else
        {
            transform.position = Vector2.MoveTowards(transform.position, hangingSpot.transform.position,
                flySpeed * Time.deltaTime);
            if (!_animator.GetBool("Flying"))
            {
                _animator.SetBool("Flying", true);
            }
        }
    }

    void ShootAttackActivator()
    {
        _animator.SetTrigger("ShootAttack");
    }

    public override void Death()
    {
        //base.Death();
        ActionsManager.InvokeAction(gameObject.name + ActionConstants.DEATH);
        canDie = true;

        state = FBBattleState.DEATH;

        StartCoroutine(FinishingGame());
    }

    IEnumerator FinishingGame()
    {
        yield return new WaitForSeconds(4.2f);

        changer.GameOver();

        yield return null;
    }

    public void InitializeShootOne()
    {
        _shootAttack.Attack(shootStartUp.position,-transform.localScale);
        shootAttackSound.Play();
    }

    public void InitializeShootTwo()
    {
        _shootAttack.Attack(shootStartDown.position, -transform.localScale);
        shootAttackSound.Play();
    }

    public void InitializeShootThree()
    {
        _shootAttack.Attack(shootStartMiddle.position, -transform.localScale);
        shootAttackSound.Play();
    }

    void BossStateExecution()
    {
        switch (state)
        {
            case FBBattleState.SPAWNING:
                if (canSpawning)
                {
                    canSpawning = false;
                    StartCoroutine(SpawningAnimation());
                }

                break;

            case FBBattleState.IDLE:

                if (canIdle)
                {
                    canFloor = true;
                    _animator.SetBool("Flying", false);
                    canIdle = false;
                    hasFloorAttacked = false;
                    StartCoroutine(Vulnerable());
                }
                else
                {
                    Vector2 toFloor = new Vector2(transform.position.x, floorSpot.transform.position.y);
                    if (Vector2.Distance(transform.position, toFloor) > 0.83f)
                    {
                        _animator.SetBool("Flying", true);
                        transform.position =
                            Vector2.MoveTowards(transform.position, toFloor, flySpeed * Time.deltaTime);
                    }
                    else
                    {
                        canIdle = false;
                        hasFloorAttacked = false;

                        canFloor = true;
                        _animator.SetBool("Flying", false);
                    }
                }

                break;

            case FBBattleState.MELEEATTACK:
                if (canMelee)
                {
                    canMelee = false;
                    MeleeAttackActivator();
                }

                break;

            case FBBattleState.FLOORATTACK:
                FloorAttackActivator();

                break;

            case FBBattleState.SHOOTATTACK:

                if (canShoot)
                {
                    canShoot = false;
                    ShootAttackActivator();
                }

                break;

            case FBBattleState.DEATH:
                if (canDie)
                {
                    Vector2 toFloor = new Vector2(transform.position.x, floorSpot.transform.position.y);



                    if (Vector2.Distance(transform.position, toFloor) > 0.83f)
                    {
                        transform.position =
                            Vector2.MoveTowards(transform.position, toFloor, flySpeed * Time.deltaTime);
                    }

                    else
                    {
                        _animator.SetBool("Walking", false);
                        _animator.SetBool("Flying", false);
                        canDie = false;
                        _animator.SetTrigger("Death");
                    }
                }

                break;
        }
    }

    void SetAttackState()
    {
        switch (attackIndex)
        {
            case 0:
                state = FBBattleState.MELEEATTACK;
                break;

            case 1:
                state = FBBattleState.FLOORATTACK;
                break;

            case 2:
                state = FBBattleState.SHOOTATTACK;
                break;
        }
    }

    void ChangeScale()
    {
        if (canChangeScale)
        {
            isRight = (_playerTransform.position.x < transform.position.x);
            float dirMultpiplier = isRight ? -1 : 1;

            Vector3 theScale = transform.localScale;
            theScale.x = dirMultpiplier;
            transform.localScale = theScale;

            if (theScale.x < 0)
            {
                theScale.x = -0.5f;
            }

            else
            {
                theScale.x = 0.5f;
            }

            theScale = new Vector3(theScale.x, 0.5f, 0.5f);
            transform.localScale = theScale;
        }
    }

    public void PutIdle()
    {
        state = FBBattleState.IDLE;
    }

    public void MeleeAttackSoundEffect()
    {
        meleeAttackSound.Play();
    }

    public void FloorAttackSoundEffect()
    {
        floorAttackSound.Play();
    }

    public void DieSoundEffect()
    {
        dieSound.Play();
    }
}
