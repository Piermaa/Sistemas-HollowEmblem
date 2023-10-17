using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirIA : MonoBehaviour
{
    [Header ("Objects")]
    public Transform bulletOrigin;
    private Rigidbody2D rb;
    private Animator animator;
    private ObjectPooler objectPooler;
    private ParticleSystem shootParticles;
    public AudioSource shootSound;

    [Header ("Patrolling Parameters")]
    public Transform[] moveSpots;
    [SerializeField] Transform next;
    public float stopTime;
    public float speed = 6;
    public float waitTime;
    private float startWaitTime = 1;
    private float speedAux;
    private int spotsIndex;
    

    [Header("Combat Parameters")]
    public float xDistance;
    public float yDistance;
    public float shootCD = 3;
    public float shootTimer;
    public bool playerAtLeft;
    public bool chasingPlayer;
    private bool shooting;
    private bool mustFall;
    private Transform playerTransform;
    private Vector3 desiredPos;

    private void Awake()
    {
        if (shootParticles == null)
        {
            shootParticles = GetComponentInChildren<ParticleSystem>();
        }
    }
    private void Start()
    {
       
  
        rb = GetComponent<Rigidbody2D>();
        objectPooler = ObjectPooler.Instance;
        animator = GetComponent<Animator>();
        speedAux = speed;
    }

    private void Update()
    {
        if (!mustFall)
        {
            if (!shooting)
            {
                if (!chasingPlayer) // SI NO ESTA PERSIGUIENDO AL JUGADOR:
                { //PATRULLA
                    next = moveSpots[spotsIndex];
                    stopTime -= Time.deltaTime;

                    if (stopTime >= 0)
                    {
                        speed = 0;
                    }
                    else
                    {
                        speed = speedAux;
                    }
                    //SE MUEVE HACIA EL SIGUIENTE SPOT
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(moveSpots[spotsIndex].transform.position.x, transform.position.y), speed * Time.deltaTime);
                    //SE REVISA QUE TAN PROXIMO ESTA AL SIGUIENTE SPOT
                    if (Vector2.Distance(transform.position, new Vector2(moveSpots[spotsIndex].position.x, transform.position.y)) < 0.3f)
                    {
                        if (waitTime <= 0)
                        {
                            //SE MODIFICA EL SIGUIENTE SPOT Y SE GIRA EL SPRITE
                            if (moveSpots[spotsIndex] != moveSpots[moveSpots.Length - 1])
                            {
                                Vector3 theScale = transform.localScale;
                                theScale.x *= -1;
                                transform.localScale = theScale;
                                spotsIndex++;
                            }
                            else
                            {
                                Vector3 theScale = transform.localScale;
                                theScale.x *= -1;
                                transform.localScale = theScale;
                                spotsIndex = 0;
                            }
                            waitTime = startWaitTime;
                        }
                        else
                        {
                            waitTime -= Time.deltaTime;
                        }

                    }
                }//(!chasing player)
                else
                {
                    if (!shooting)
                    {
                        shootTimer = (shootTimer > 0) ? (shootTimer - Time.deltaTime) : 0;
                        ChasePlayer();
                    }
                }
            }//(!shooting)
            else
            {
                rb.velocity = Vector2.zero;
            }
        }//(!mustFall)
        {
            
        }
    }

    public void MustChasePlayer(Transform playerTr)
    {
        playerTransform = playerTr;
        chasingPlayer = true;
    }
    void ChasePlayer()
    {
        playerAtLeft = (playerTransform.position.x < transform.position.x) ? true : false;
        float dirMultpiplier = playerAtLeft ? 1 : -1;

        desiredPos = playerTransform.position + new Vector3(xDistance*dirMultpiplier, yDistance);
        transform.position = Vector2.MoveTowards(transform.position, desiredPos,4*Time.deltaTime);

        //HAY ALGUNA FORMA DE NO EJECUTAR ESTO A CADA FRAME Y EJECUTARLO SOLO CUANDO EL VALOR CAMBIA????
        Vector3 theScale = transform.localScale;
        theScale.x = dirMultpiplier;
        transform.localScale = theScale;
     
        if (shootTimer<=0)
        {
           // print("shoot");
            shootTimer = shootCD;
            StartShoot();
        }
    }
    public void StopWalking(float time) 
    {
        stopTime = time;
    }
    public void Shot()
    {
        shootParticles.Play();
     //   objectPooler.SpawnFromPool("Bullet", bulletOrigin.position, Quaternion.Euler(playerTransform.position - bulletOrigin.position), (playerTransform.position));//- transform.position);
    }
    public void PlayShotSund()
    {
        shootSound.Play();
        print("shotsound");
    }
    public void StartShoot()
    {
        animator.SetTrigger("Shot");
        shooting = true;
    }

    public void FinishShot()
    {
        shooting = false;
    }

    public void FallToDeath()
    {
        gameObject.layer = 0;
        mustFall = true;
        rb.gravityScale = 1;
        rb.AddForce(Vector2.down*3, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (mustFall)
        {
            if (collision.gameObject.layer == 6 || collision.gameObject.layer == 22)
            {
                gameObject.SetActive(false);
            }
        }
    }
  
}
