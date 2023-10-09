using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
public class HealthController : MonoBehaviour
{
    [Header("Classes")]
    [SerializeField] SpriteRenderer sprite;
    public AudioSource takeDamageSound;
    public Material takingDamageMaterial;
    public GameObject dropItemPrefab;
    Material baseMaterial;
    private UIHealth uiHealth;
    public ParticleSystem takeDamageParticles;

  [Header("Bool")]
    public bool inmune = false;
    public bool white;
    public bool died;

    bool takingDamage;
    [Header("Int")]
    public int healthPoints = 100;
    public int maxHealth;
 
    public float playerInmunity = 1;

    [Header("Events")]
    public UnityEvent OnHealthAdd;
    public UnityEvent DieEvent;

  

    private void Awake()
    {
        if (sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        if (sprite == null)
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
        }
            uiHealth = FindObjectOfType<UIHealth>();
        
    }

    private void Start()
    {
        if (sprite!=null)
        {
            baseMaterial = sprite.material;

        }
   
        if (maxHealth != 0) FullHeal();
    }

    private void Update()
    {
        if (maxHealth!=0)
        {
            if(healthPoints>maxHealth)
            {
                healthPoints = maxHealth;
            }
        }

        if (healthPoints <= 0)
        {
            Death();
        }
    }
    IEnumerator DamageTaken()
    {
        takingDamage = true;
        yield return new WaitForSeconds(0.1f*playerInmunity);
        sprite.material = baseMaterial;
        takingDamage = false;
    }
    /// <summary>
    /// Reduces healthpoints and starts a coroutine that changes the target's sprite color
    /// </summary>
    /// <param name="damage">Amount of damage</param>
    public void TakeDamage(int damage)
    {
        takeDamageSound.PlayOneShot(takeDamageSound.clip);
        if (!inmune && healthPoints>0 &&!takingDamage)
        {   
            healthPoints -= damage;
            uiHealth.hasTakeDamage = true;

            sprite.material = takingDamageMaterial;

            //Color col = sprite.color;
            //var red = new Color(255, 0, 0);

            //sprite.color = red;
            if (takeDamageParticles!=null)
            {
                //float xMult = gameObject.transform.localScale.x;
                //takeDamageParticles.velocityOverLifetime.orbitalX *= xMult;
                takeDamageParticles.Play();
            }
            StartCoroutine(DamageTaken());
        }
    }
    public void Heal(int hpAdded)
    {
        healthPoints += hpAdded;
    }

    public void FullHeal()
    {
        healthPoints = maxHealth;
    }

    public void AddMaxHealth(int healthAdded)
    {
        maxHealth+=healthAdded;
        FullHeal();
        OnHealthAdd.Invoke();
    }

    public void Death()
    {
        if (!died)
        {
            if (DieEvent != null)
            {
                DieEvent.Invoke();
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
     
    }
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    public void Drop()
    {
        int ammount = Random.Range(0, 5);
        RamdomizeItem(dropItemPrefab,ammount);
    }
    public void RamdomizeItem(GameObject itemToDrop,int cuantity)
    {
        if (cuantity>0)
        {
            int type = Random.Range(0, 2);
            switch (type)
            {
                case 0:
                    CreateDrop("Ammo", cuantity);
                    break;
                case 1:
                    CreateDrop("Heal", cuantity);
                    break;
            }
        }
       
    }
    void CreateDrop(string itemName,int cuantity)
    {
        GameObject drop = Instantiate(dropItemPrefab,this.transform.position,this.transform.rotation);
        drop.TryGetComponent<PickupableItem>(out var item);
        item.itemName = itemName;
        item.amount = cuantity;
    }
    public void DropItem(GameObject itemToDrop, int cuantity,string type)
    {

    }

    public void AnimatedDeath()
    {
        died = true;
        gameObject.TryGetComponent<Animator>(out var anim);
        if(anim!=null)
        {
            anim.SetTrigger("Death");
        }
        gameObject.TryGetComponent<BasicIA>(out var bia);
        if (bia!=null)
        {
            bia.enabled = false;
            //Destroy(bia);
        }
    }
}
