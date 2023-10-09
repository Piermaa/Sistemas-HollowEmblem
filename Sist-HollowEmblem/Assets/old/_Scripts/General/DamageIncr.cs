using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIncr : MonoBehaviour
{
    public int dmgToAdd;

    public GameObject textFeedback;

    public GameObject miniMapIcon;

    private SpriteRenderer sr;

    private void Start()
    {
        textFeedback.SetActive(false);
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (collision.gameObject.CompareTag("Player"))
        {
            
            collision.gameObject.TryGetComponent<PlayerCombat>(out PlayerCombat playerCombat);

            playerCombat.damage+= dmgToAdd;

            StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        textFeedback.SetActive(true);
        sr.enabled = false;
        miniMapIcon.SetActive(false);

        yield return new WaitForSeconds(1.5F);

        textFeedback.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        this.gameObject.SetActive(false);
    }
}

