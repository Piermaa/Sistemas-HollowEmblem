using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    int healingAmount=2;

    bool canHeal;
    public GameObject healer;
    public float healCD;
    float healTimer;

    public GameObject textFeedback;


    void Update()
    {
        healTimer -= Time.deltaTime;
        if(healTimer<0)
        {
            canHeal = true;
        }
        healer.SetActive(canHeal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&canHeal)
        {
           
                HealthController health;
                collision.TryGetComponent<HealthController>(out health);
      
                canHeal = false;
                health.healthPoints += healingAmount;
                healTimer = healCD;
                print("playerHeaLED:");

                StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        textFeedback.SetActive(true);

        yield return new WaitForSeconds(1.5F);

        textFeedback.SetActive(false);
    }
}
