using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamager : MonoBehaviour
{
    HealthManager healthManager;


    private void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
    }

    public int damage=1;

    IEnumerator parrySuccessful()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Parry"))
        {
           
            healthManager.SetInmunity();
            StartCoroutine(parrySuccessful());
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.TryGetComponent<HealthController>(out var playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
        }
        
    }
}
