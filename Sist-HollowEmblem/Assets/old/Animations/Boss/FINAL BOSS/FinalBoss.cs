using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    ObjectPooler objectPooler;

    [Tooltip("Added in destruction order (first 1, then 4,3,2)")]
    [SerializeField] GameObject[] platforms;
    public Transform shootTransform;
    Animator animator;
    HealthController health;
    int maxHealth;

    public int pos;

    public bool bossInRight;
    public bool shooting;
    public bool successfullyMoved;

    float rangedAttackTimer;
    public float rangedAttackCooldown;

    public string[] ammo;

    [SerializeField] AudioSource shot;
    [SerializeField] AudioSource slam;
    [SerializeField] AudioSource a;
    // Start is called before the first frame update

    public enum BossPos
    {
        Pos1, Pos2, Pos3, Pos4
    }
    public BossPos bossPos;
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        successfullyMoved = true;
        health = GetComponent<HealthController>();
        animator = GetComponent<Animator>();
        bossPos = BossPos.Pos1;
        maxHealth = health.healthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        BossMovement();
        BossShooting();
       
    }

    void BossShooting()
    {
        rangedAttackTimer -= Time.deltaTime;
        if (rangedAttackTimer<0)
        {
            Shoot();
            rangedAttackTimer = rangedAttackCooldown;
        }
    }

    void Shoot()
    {
        // how i set the rotation is very possibly incorrect because I do not know wich is left or right
        StartCoroutine(PlaySoundDelay(shot));
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Quaternion invertedRotation = Quaternion.Euler(new Vector3(0, 0, -1));
        if (successfullyMoved)
        {

            Quaternion rot;
            if (bossInRight)
            {
                rot = rotation;
            }
            else
            {
                rot = invertedRotation;
            }

            rangedAttackTimer = rangedAttackCooldown;


            StartCoroutine(Shooting(rot));
            print("spawning bullet");


        }


    }
    IEnumerator PlaySoundDelay(AudioSource sound)
    {
        yield return new WaitForSeconds(0.2f);
        sound.Play();
    }

    IEnumerator Shooting(Quaternion rotation)
    {
        shooting = true;
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.45f);
        foreach (string ammunition in ammo)
        {
            objectPooler.SpawnFromPool(ammunition, shootTransform.position, rotation);
        }
        shooting = false;
    }

    void BossMovement()
    {
        if (successfullyMoved)
        {
            if (health.healthPoints<16 && bossPos==BossPos.Pos1)
            {
                bossPos = BossPos.Pos2;
                ChangePos(1);
            }
            if (health.healthPoints < 12 && bossPos == BossPos.Pos2)
            {
                bossPos = BossPos.Pos3;
                ChangePos(2);
            }
            if (health.healthPoints < 8 && bossPos == BossPos.Pos3)
            {
                bossPos = BossPos.Pos4;
                ChangePos(3);
            }
            if (health.healthPoints<4 && bossPos==BossPos.Pos4)
            {
                ChangePos(4);
            }
            switch (health.healthPoints)
            {
                case 16:
                    
                
                    break;

                case 12:
              
                    break;

                case 8:
             
                    break;

                case 4:
             
                    break;
            }
        }
    }

    void ChangePos(int newPos)
    {
       
        animator.SetInteger("MoveTo",newPos);
        StartCoroutine(DestroyPlatform(platforms[newPos-1]));
    }

    IEnumerator DestroyPlatform(GameObject platform)
    {
        slam.Play();
        yield return new WaitForSeconds(1f);
        platform.TryGetComponent<Animator>(out var dAnimator);
        dAnimator.SetTrigger("Destroy");
        successfullyMoved = false;
        rangedAttackCooldown -= 0.5f;
    }
}
