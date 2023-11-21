using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Classes")]
    ObjectPooler objectPooler;
    public GameObject abUnlocker;
    [SerializeField] Animator spikesAnimator;
    [SerializeField] GameObject spikesOnBody;
    [SerializeField] Transform shootTransform;
    //[SerializeField] GameObject tutorial;
    Animator animator;
     
    [Header("Bools")]
    bool shooting;
    public bool bossInRight;

    [Header("Ints")]
    int rangedAttackCounter;
    int changeSideUpCounter;
    public int maxChangeSideUp;
    public int maxRangedAttacks;

    [Header("Floats")]
    public float rangedAttackCooldown;
    public float changePosCooldown;
    public float lowAttackCooldown;
    float rangedAttackTimer;
    float changePosTimer;
    float lowAttackTimer;

    [Header("Strings")]
    public string[] ammo;

    [Header("Sounds")]
    [SerializeField] AudioSource shot;
    [SerializeField] AudioSource intoFlor;
    [SerializeField] AudioSource intoSlime;

    public enum BossState
    {
        Solid, Liquid
    }

    public BossState bossState;

    private void Start()
    {
        rangedAttackTimer = rangedAttackCooldown;
        animator = GetComponent<Animator>();
        objectPooler = ObjectPooler.Instance;
    }

    private void Update()
    { 
        if(bossState==BossState.Liquid)
            rangedAttackTimer = rangedAttackCooldown;

        if (bossState != BossState.Liquid) 
        {
            rangedAttackTimer -= Time.deltaTime;
        }

        if(rangedAttackTimer<0)
        {
            Shoot();
        }

        if (rangedAttackCounter >= maxRangedAttacks && bossState != BossState.Liquid &&!shooting)
        {
            if (changeSideUpCounter < maxChangeSideUp)
            {
                ChangeSide();
            }
            else
            {
                LowAttack();
            }
        }
    }

    /// <summary>
    /// Triggers the attack from underground
    /// </summary>
    void LowAttack()
    {
        if (bossInRight)
        {
            animator.SetTrigger("LowLeft");
            spikesAnimator.SetTrigger("Left");
        }
        else
        {
            animator.SetTrigger("LowRight");
            spikesAnimator.SetTrigger("Right");
        }
        StartCoroutine(PlaySoundDelay(intoFlor));
        rangedAttackCounter = 0;
        changeSideUpCounter= 0;
    }

    void ChangeSide()
    {
        StartCoroutine(PlaySoundDelay(intoSlime));
        if (bossInRight)
        {
            animator.SetTrigger("Left");
        }
        else
        {
            animator.SetTrigger("Right");
        }
        changeSideUpCounter++;
        rangedAttackCounter = 0;
    }

    
    void Shoot()
    {

        StartCoroutine(PlaySoundDelay(shot));
        // how i set the rotation is very possibly incorrect because I do not know wich is left or right
        Quaternion rotation =Quaternion.Euler( new Vector3(0,0,0));
        Quaternion invertedRotation = Quaternion.Euler(new Vector3(0, 0, -1));
        if (bossState != BossState.Liquid)
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

            StartCoroutine(Shooting(rot));
            rangedAttackTimer = rangedAttackCooldown;
            rangedAttackCounter++;
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
        animator.SetTrigger("Shot");
        yield return new WaitForSeconds(0.45f);
        foreach (string ammunition in ammo)
        {
            objectPooler.SpawnFromPool(ammunition, shootTransform.position, rotation);
        }
        shooting = false;
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


            Vector3 dir = collision.transform.position- transform.position;
            dir.Normalize();
            dir = new Vector3(dir.x*50,2);
            rb.AddForce(dir, ForceMode2D.Impulse);

            health.TakeDamage(1);
            

        }
    }

    public void Death()
    {
        var drop = Instantiate(abUnlocker, transform.position,transform.rotation);
        drop.TryGetComponent<AbilityUnlocker>(out var ability);
        ability.unlockedAb = "Slime";

        //if (tutorial!=null)
        //{
        //    tutorial.SetActive(true);
        //}
                {
            SceneChanger.Instance.GameOver();
        }
    }
}
