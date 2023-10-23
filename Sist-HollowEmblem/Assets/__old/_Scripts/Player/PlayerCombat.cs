using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 //ESTE ES EL NECESARIO PARA USAR LUCES 2D !!!!!!1!!!
public class PlayerCombat : MonoBehaviour
{
    public LayerMask enemyLayer;
    Vector2 attackDirection;
    public GameObject pistolUI;


    [Header("Objects")]
    ObjectPooler objectPooler;
    Legacy_PlayerInventory inventory;
    [SerializeField] PlayerSounds sounds;
    Rigidbody2D rb;
    Animator animator;
    CharacterController2D controller;
    ParticleSystem bulletShootParticles;
    [SerializeField] GameObject bulletShootLights;
    private UnityEngine.Rendering.Universal.Light2D shootLightSpread;
    private MapInput cortana;
    private IEnumerator reloadingC;
    [Header("Transforms")]
    public Transform playerCenter; //saves the position of the player center, this to attack from there when liquid
    public Transform attackPosition; // saves the transform of the normal attack in case of changing to liquid
    public Transform shootStart;
    public Transform attackPoint; // the transform used when attacking
    public Transform[] attackDirections; //0 forward 1up 2down
    public Transform[] shootDirections;

    [Header("Bools")]
    public bool canAttack;
    public bool aiming;
    public bool reloading;
    public bool showingInventory;
    public bool canShoot;
    [SerializeField]bool mustTurnOff;

    [Header("Floats")]
    public float attackRange = .8f;

    [Header("Int")]
    public int damage;
    public int shootDamage;
    public int maxAmmo = 10;
    public int currentAmmo;
    public List<GameObject> ammo;

    public enum DirectionsToAttack
    {
        Up,Down,Front
    }
    public DirectionsToAttack directionsToAttack;

    private void Start()
    {
        cortana = MapInput.Cortana;
        objectPooler = ObjectPooler.Instance;
        pistolUI.SetActive(false);
        shootLightSpread = bulletShootLights.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
      
        bulletShootParticles = bulletShootLights.GetComponentInChildren<ParticleSystem>();
        bulletShootLights.SetActive(false);
        inventory = Legacy_PlayerInventory.Instance;
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentAmmo = maxAmmo;
    } 
    public void Update()
    {
        SetAttackDirection();
        if (cortana.state==ShowStates.HIDING)
        {
            if (canShoot)
            {
                pistolUI.SetActive(true);
                Aim();

                if (Input.GetKeyDown(KeyCode.R))
                {
                    Reload();
                }
            }

            if (Input.GetButtonDown("Attack") && canAttack)
            {
                Attack();
            }

            if (reloading)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        

        if (mustTurnOff)
        {
            StartCoroutine(LightSpread());
        }

        //Debug.DrawRay(shootStart.position, Vector2.right*100,Color.red);
        Debug.DrawRay(shootStart.position, shootStart.TransformDirection(Vector2.left) * 100, Color.red);
    }

    void SetAttackDirection()
    {
        float y = Input.GetAxis("Vertical");

        if(y==0)
        {
            directionsToAttack = DirectionsToAttack.Front;
    
            shootStart.position = shootDirections[0].position;
            shootStart.rotation = shootDirections[0].rotation;
            
        }
        if(y>0)
        {
            directionsToAttack = DirectionsToAttack.Up;
            
            shootStart.position = shootDirections[1].position;
            shootStart.rotation = shootDirections[1].rotation;
 
        }
    
        if(y<0)
        {
            directionsToAttack = DirectionsToAttack.Down;
            shootStart.position = shootDirections[2].position;
            shootStart.rotation = shootDirections[2].rotation;
           
        }
    }


    public void AttackFront()
    {
        objectPooler.SpawnFromPool("PlayerAttack", attackDirections[0].position, attackDirections[0].rotation, rb,transform.localScale);
    }

    public void AttackUp()
    {
        objectPooler.SpawnFromPool("PlayerAttackUp", attackDirections[1].position, attackDirections[1].rotation, rb, transform.localScale);
    }

    public void AttackDown()
    {
        objectPooler.SpawnFromPool("PlayerAttackDown", attackDirections[2].position, attackDirections[2].rotation, rb, transform.localScale);
    }

    /// <summary>
    /// Attacks using overlapcricleall, triggers the correct animation and inflict damage on enemies
    /// </summary>
    void Attack()
    {
        
        animator.SetBool("Jump", false);
        switch (directionsToAttack)
        {
            case DirectionsToAttack.Front:
                sounds.PlaySound(sounds.attack);
                animator.SetTrigger("AttackFront");
                break;
            case DirectionsToAttack.Down:
                
                if (!controller.CheckGround())
                {
                    sounds.PlaySound(sounds.attack);
                    animator.SetTrigger("AttackDown");
                }
      
                break;
            case DirectionsToAttack.Up:
                sounds.PlaySound(sounds.attack);
                animator.SetTrigger("AttackUp");
                break;
        }
    }

    void Aim()
    {
        if (Input.GetMouseButton(1) && controller.CheckGround())
        {
            
            canAttack = false;
            aiming = true;

            switch (directionsToAttack)
            {

                case DirectionsToAttack.Front:
                    animator.SetTrigger("AimFront");
                    break;
                case DirectionsToAttack.Down:
                    animator.SetTrigger("AimDown");
                    break;
                case DirectionsToAttack.Up:
                    animator.SetTrigger("AimUp");
                    break;
            }

            Shoot();
        }
        else if (!reloading)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            aiming = false;
        }
        animator.SetBool("Aiming", aiming);
    }
    void Shoot()
    {
        RaycastHit2D hit;

        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            ShootFX();
            hit = Physics2D.Raycast(shootStart.position, shootStart.TransformDirection(Vector2.left), 50, enemyLayer); // the bug was the direction, as the shootstart is rotated... (1/2)
            //depending of the aimDirection transform we have to use TransformDirection(new Vector(direction)), also enemyLayer as it works as int it worked as distance
            currentAmmo--;

            UpdateUI();

            if (hit)
            {
                Debug.Log(hit.collider.name);   
                hit.collider.TryGetComponent < HealthController >(out var health);
                if(health!=null)
                {
                    health.TakeDamage(shootDamage);
                }
                
            }
        }
    }
    void ShootFX()
    {
        sounds.PlaySound(sounds.shoot,true);
        bulletShootLights.SetActive(true);
        bulletShootParticles.Play();
        shootLightSpread.intensity = 1.4f;
        mustTurnOff = true;
    }


    IEnumerator LightSpread()
    {
        for (float i = shootLightSpread.intensity; i > 0; i -= Time.deltaTime*20)
        {
            shootLightSpread.intensity = i;
            yield return null;
        }
        mustTurnOff = false;
        shootLightSpread.intensity = 0;
        yield return new WaitForSeconds(0.1f);
       
    }
    public void Reload()
    {
        if ( controller.CheckGround() && maxAmmo>currentAmmo && !reloading &&reloadingC==null)
        {
            reloadingC = Reloading();
            StartCoroutine(reloadingC);
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < ammo.Count; i++)
        {
            if (i < currentAmmo)
            {
                ammo[i].SetActive(true);
            }
            else
            {
                ammo[i].SetActive(false);
            }
        }
    }

    IEnumerator Reloading()
    {
      
        bool hasAmmoOnInventory= inventory.GetAmmoFromInventory(true) != 0;
        //int actualAmmo = currentAmmo;
        if (hasAmmoOnInventory)
        {
            reloading = true;
            sounds.PlaySound(sounds.reload);
            animator.SetTrigger("Reload");
            Debug.Log("Initial ammo from inventory:"+inventory.GetAmmoFromInventory(true));
            yield return new WaitForSeconds(0.9f);
            while (hasAmmoOnInventory)
            {
                print("has ammo on inventory:"+hasAmmoOnInventory);
                currentAmmo += inventory.GetAmmoFromInventory(false);
                hasAmmoOnInventory = inventory.GetAmmoFromInventory(true) > 0;
                UpdateUI();
            }
            UpdateUI();
            reloadingC = null;
            reloading = false;
        }
        else
        {
            reloading = false;
            reloadingC = null;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, shootStart.TransformDirection(Vector3.forward)*100);
    }
}
