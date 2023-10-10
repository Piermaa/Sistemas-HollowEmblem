using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBossController : MonoBehaviour
{
    public string[] ammo;
    public string[] downAmmo;
    public string[] farAmmo;

    [SerializeField] GameObject tutorial;
    [SerializeField] float rangedAttackTimer;
    float changePosTimer;

    float restTimer;

    public GameObject abUnlocker;
    [SerializeField] float rangedAttackCooldown = 0.5f;
    public float changePosCooldown;
    public float restCooldown;



    [SerializeField] int changeSideCounter;
    public int maxChangeSide;

    Animator animator;
    ObjectPooler objectPooler;

    [SerializeField] GameObject[] defenses;
    [SerializeField] GameObject spikesOnBody;
    [SerializeField] Transform shootTransform;
    [SerializeField] Transform shootDownTransform;

    [SerializeField] AudioSource jump;
    [SerializeField] AudioSource shoot;


    public bool bossInRight;

    public enum PlayerPos
    {
        Left, Right, Middle
    }
    public PlayerPos playerPos;
    public enum BossState
    {
        Shooting, Resting, Waiting, Moving
    }

    public BossState bossState;

    public enum BossPosition
    {
        Left, Right, Middle
    }
    public BossPosition bossPosition;
    private void Start()
    {

        restTimer = restCooldown;
        animator = GetComponent<Animator>();
        objectPooler = ObjectPooler.Instance;
    }

    private void Update()
    {
        if (bossState == BossState.Moving)
        {
            rangedAttackTimer = rangedAttackCooldown;
        }

        //shooting -------------------------------------------
        if (bossState == BossState.Shooting) // si el jefe esta disparando
        {
            rangedAttackTimer -= Time.deltaTime; // se le resta el tiempo al timer
        }

        if (rangedAttackTimer < 0) //si el tiempo del timer es menor a cero
        {
            Shoot(); //jefe dispara
        }
        //changin sides-------------------.---------.--.-.-.-.-.-.-.-.-.-.-.-.
        if (bossState == BossState.Waiting && changeSideCounter < maxChangeSide) //si el jefe està esperando y su contador de cambios de lado es menor al màximo, se le resta tiempo el timer de cambio de lado
        {
            changePosTimer -= Time.deltaTime;
        }

        if (changePosTimer < 0)
        {
            ChangeSide();
            changePosTimer = changePosCooldown;
            foreach (GameObject wall in defenses)
            {
                wall.SetActive(true);
            }

        }

        //Rest-------------------------------------------------------

        if (bossState == BossState.Waiting && changeSideCounter == maxChangeSide)
        {
            animator.SetTrigger("Rest");
            bossState = BossState.Resting;
        }

        if (bossState == BossState.Resting)
        {
            restTimer -= Time.deltaTime;
            foreach (GameObject wall in defenses)
            {
                wall.SetActive(false);
            }
        }
        if (restTimer < 0)
        {
            animator.SetTrigger("Idle");
            bossState = BossState.Waiting;
            changeSideCounter = 0;

            restTimer = restCooldown;
        }
    }

    IEnumerator WaitForSideChange()
    {
        yield return new WaitForSeconds(2);
        changeSideCounter++;
    }

    void ChangeSide()
    {
        switch (bossPosition)
        {
            case BossPosition.Left:
                animator.SetTrigger("Right");
                break;

            case BossPosition.Right:
                animator.SetTrigger("Left");
                break;
        }
        StartCoroutine(JumpSoundDelay());
        StartCoroutine(WaitForSideChange());
    }


    void Shoot()
    {
        // how i set the rotation is very possibly incorrect because I do not know wich is left or right
        shoot.Play();
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Quaternion invertedRotation = Quaternion.Euler(new Vector3(0, 0, -1));

        Quaternion rot = Quaternion.identity;
        if (bossState == BossState.Shooting)
        {

            switch (bossPosition)
            {
                case BossPosition.Left://-------------------------------------------------------------------------------------------------------------------------

                    switch (playerPos)
                    {
                        case PlayerPos.Left://---------------------------------------------------------DOWN
                            rot = rotation;
                            objectPooler.SpawnFromPool(downAmmo[0], shootDownTransform.position, invertedRotation);
                            objectPooler.SpawnFromPool(downAmmo[1], shootDownTransform.position, rot);
                            objectPooler.SpawnFromPool(downAmmo[2], shootDownTransform.position, rot);

                            break;

                        case PlayerPos.Middle:
                            rot = invertedRotation;
                            foreach (string ammunition in ammo)
                            {
                                objectPooler.SpawnFromPool(ammunition, shootTransform.position, rot);
                            }
                            break;

                        case PlayerPos.Right:
                            rot = invertedRotation;
                            foreach (string ammunition in farAmmo)
                            {
                                objectPooler.SpawnFromPool(ammunition, shootTransform.position, rot);
                            }
                            break;
                    }
                    break;

                case BossPosition.Middle://-------------------------------------------------------------------------------------------------------------------------

                    switch (playerPos)
                    {

                        case PlayerPos.Left:
                            rot = rotation;
                            foreach (string ammunition in farAmmo)
                            {
                                objectPooler.SpawnFromPool(ammunition, shootTransform.position, rot);
                            }
                            break;
                        case PlayerPos.Middle://---------------------------------------------------------DOWN

                            foreach (string ammunition in downAmmo)
                            {
                                objectPooler.SpawnFromPool(ammunition, shootDownTransform.position, rot);
                            }
                            break;
                        case PlayerPos.Right:
                            rot = invertedRotation;
                            rot = invertedRotation;
                            foreach (string ammunition in farAmmo)
                            {
                                objectPooler.SpawnFromPool(ammunition, shootTransform.position, rot);
                            }
                            break;
                    }
                    break;

                case BossPosition.Right: //-------------------------------------------------------------------------------------------------------------------------

                    switch (playerPos)
                    {
                        case PlayerPos.Left:
                            rot = rotation;
                            foreach (string ammunition in farAmmo)
                            {
                                objectPooler.SpawnFromPool(ammunition, shootTransform.position, rot);
                            }
                            break;

                        case PlayerPos.Middle:
                            rot = rotation;
                            foreach (string ammunition in ammo)
                            {
                                objectPooler.SpawnFromPool(ammunition, shootTransform.position, rot);
                            }

                            break;

                        case PlayerPos.Right: //---------------------------------------------------------DOWN

                            foreach (string ammunition in downAmmo)
                            {
                                objectPooler.SpawnFromPool(ammunition, shootDownTransform.position, rot);
                            }
                            break;

                    }
                    break;
            }
            rangedAttackTimer = rangedAttackCooldown;
        }

    }


    public void Death()
    {
        var drop = Instantiate(abUnlocker, transform.position, transform.rotation);
        drop.TryGetComponent<AbilityUnlocker>(out var ability);
        ability.unlockedAb = "Dash";


        SceneChanger.Instance.GameOver();
        //tutorial.SetActive(true);

    }
    IEnumerator JumpSoundDelay()
    {
        jump.Play();
        yield return new WaitForSeconds(0.5f);
    
 
        jump.Play();
    }
    IEnumerator ShowSpikes()
    {
        spikesOnBody.SetActive(true);
        yield return new WaitForSeconds(1);
        spikesOnBody.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ShowSpikes());

            Rigidbody2D rb;
            HealthController health;
            collision.gameObject.TryGetComponent<HealthController>(out health);
            collision.gameObject.TryGetComponent<Rigidbody2D>(out rb);


            Vector3 dir = collision.transform.position - transform.position;
            dir.Normalize();
            dir = new Vector3(dir.x * 50, 2);
            rb.AddForce(dir, ForceMode2D.Impulse);

            health.TakeDamage(1);

            print("PlayerAttacked");
        }
    }


}
