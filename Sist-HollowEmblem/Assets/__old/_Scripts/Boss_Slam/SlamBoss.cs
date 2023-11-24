using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamBoss : BossEnemy
{
    private HealthController health;
    private Rigidbody2D rb;
    private ObjectPooler objectPooler;
    [SerializeField] private GameObject abUnlocker;

    private bool playedJumpSound;
    private bool mustSlam;
    private bool mustReposition;
    private int maxHealth;

    [SerializeField] private VulcanAttack _vulcanAttack;
    [SerializeField] private float slamRange=5;
    [SerializeField] private int damage;
    [SerializeField] private GameObject slamFX;
    [SerializeField] private string ammunition;
    [SerializeField] private float slamSpeed;
    [SerializeField] private float repositionAcc;
    [SerializeField] private float slamCoolDown;
    [SerializeField] private AudioSource slam;
    [SerializeField] private AudioSource jump;
    private float slamTimer;

    public Transform[] positionsTransforms;
    private Vector3[] positions = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
    private Vector3[] slamPositions = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    private Vector3 newPos;
    private Vector3 slamPos;

    const string RIGHT = "Right";
    const string LEFT = "Left";
    const string MIDDLE = "Middle";
    const string MIDDLERIGHT = "MiddleRight";
    const string MIDDLELEFT = "MiddleLeft";

    [SerializeField] private GameObject tutorial;
    public enum BossPosition
    {
        Left,
        Middle,
        MiddleRight,
        MiddleLeft,
        Right
    }

    public BossPosition bossPosition;

    protected override void Awake()
    {
        base.Awake();
        _vulcanAttack.InitializeEnemyAttack(gameObject);
        _currentPhase = _bossPhases[0];
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            positions[i] = positionsTransforms[i].position;
            slamPositions[i] = new Vector3(positions[i].x, positions[i].y-5);
        }

        objectPooler = ObjectPooler.Instance;
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthController>();
        maxHealth = health.healthPoints;
    }

    protected override void Update()
    {
        base.Update();

        if(health.healthPoints > maxHealth*7.5f/10)
        {
            if (slamTimer < 0)
            {
                StartSlam(1);
            }
        }

        if (health.healthPoints < maxHealth * 7.5f / 10 && health.healthPoints > maxHealth * 5 / 10)
        {
            slamCoolDown = 5;

            if (slamTimer < 0)
            {
                StartCoroutine(TwoSlams());
            }
        }

        if (health.healthPoints < maxHealth * 2.5f / 10)
        {
            if (slamTimer < 0)
            {
                StartCoroutine(ThreeSlams());
            }

            slamCoolDown = 4;
        }

        // slamSpeed += repositionAcc * Time.deltaTime;
        if (!mustSlam && !mustReposition)
        {
            slamTimer -= Time.deltaTime;
        }

        Reposition();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.collider.tag=="Ground"|| collision.collider.tag == "DestructibleGround")&& mustSlam)
        {
            Shot();
            mustSlam = false;
            StartCoroutine(DisplayingSlam(collision));
        }
    }

    void Reposition()
    {
        if (mustReposition)
        {
            if (!playedJumpSound)
            {
                jump.Play();
                playedJumpSound = true;
            }

            _animator.SetBool("Jump", true);
            transform.position = Vector2.MoveTowards(transform.position, newPos, Time.deltaTime *
                (slamSpeed + 1 / 2 * repositionAcc * Time.deltaTime * Time.deltaTime));

            if (Vector2.Distance(transform.position, newPos) < 0.05f)
            {
                mustReposition = false;
                StartCoroutine(WaitingToSlam());
            }
        }
    }
    IEnumerator DisplayingSlam(Collision2D collision)
    {
        _animator.SetBool("Jump", false);
        _animator.SetTrigger("Slam");
        slam.Play();
        playedJumpSound = false;
        yield return new WaitForSeconds(0.1f);

        if (collision.collider.tag == "DestructibleGround")
        {

            collision.gameObject.SetActive(false);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(slamFX.transform.position.x, slamFX.transform.position.y) , slamRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player")) 
            {
                collider.GetComponent<IDamageable>().TakeDamage(damage);
            }
        }
     
    }

    IEnumerator WaitingToSlam()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.3f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.AddForce(Vector2.down*slamSpeed,ForceMode2D.Impulse);

        mustSlam = true;
    }

    IEnumerator TwoSlams()
    {
        StartSlam(1);
        slamTimer = 2;
        yield return new WaitForSeconds(1);
        StartSlam(1);
    }

    IEnumerator ThreeSlams()
    {
        StartSlam(1);
        slamTimer = 2;
        yield return new WaitForSeconds(1);
        StartSlam(1);
        slamTimer = 2;
        yield return new WaitForSeconds(1);
        StartSlam(1);
    }
    void StartSlam(int repetitions)
    {
        for(int i = 0; i < repetitions; i++)
        {
            string dir = DecideSlam();

            switch (dir)
            {
                case LEFT:
                    bossPosition = BossPosition.Left;
                    newPos = positions[0];
                    slamPos = slamPositions[0];
                    print(positions[0]);
                    break;

                case MIDDLELEFT:
                    bossPosition = BossPosition.MiddleLeft;
                    newPos = positions[1];
                    slamPos = slamPositions[1];
                    print(positions[1]);
                    break;

                case MIDDLE:
                    bossPosition = BossPosition.Middle;
                    newPos = positions[2];
                    slamPos = slamPositions[2];
                    print(positions[2]);
                    break;

                case MIDDLERIGHT:
                    bossPosition = BossPosition.MiddleRight;
                    newPos = positions[3];
                    slamPos = slamPositions[3];
                    print(positions[3]);
                    break;

                case RIGHT:
                    bossPosition = BossPosition.Right;
                    newPos = positions[4];
                    slamPos = slamPositions[4];
                    print(positions[4]);
                    break;
            }
            
            mustReposition = true;
        }

        slamTimer = slamCoolDown;
     
    }
    string DecideSlam()
    {
        string finalDirection = null;
        do
        {
            int dir = Random.Range(1, 6);
            switch (dir)
            {
                case 1:
                    finalDirection = LEFT;
                    break;
                case 2:
                    finalDirection = MIDDLELEFT;
                    break;
                case 3:
                    finalDirection = MIDDLE;
                    break;
                case 4:
                    finalDirection = MIDDLERIGHT;
                    break;
                case 5:
                    finalDirection = RIGHT;
                    break;
            }
        } while (bossPosition.ToString() == finalDirection);
        print(finalDirection);
        return finalDirection;
    }

    void Shot()
    {
        _vulcanAttack.Attack(transform.position, Vector3.up);
    }

    public override void Death()
    {
        var drop = Instantiate(abUnlocker, transform.position, transform.rotation);
        drop.TryGetComponent<AbilityUnlocker>(out var ability);
        ability.unlockedAb = "Slam";

        tutorial.SetActive(true);
    }
}
