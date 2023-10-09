using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    public GameObject[] emptyHearts;

    public List<Image> healthSprites = new List<Image>();
    [SerializeField] HealthController healthController;

    private Slider healthSlider;
    [SerializeField] Slider secondHealthSlider;

    public bool hasTakeDamage;
    public bool isBoss;


    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        hasTakeDamage = false;
        UpdateMaxHealth();
        secondHealthSlider.value = healthController.healthPoints;
    }

    private void Update()
    {
        UpdateHealth();
        TakingDamage();
    }

    public void UpdateMaxHealth()
    {
        //healthSprites.Clear();
        //for (int i = 0; i < healthController.maxHealth; i++)
        //{
        //    emptyHearts[i].SetActive(true);
        //    healthSprites.Add(emptyHearts[i].GetComponent<Image>());
        //}

        healthSlider.maxValue = healthController.maxHealth;
        secondHealthSlider.maxValue = healthController.maxHealth;
    }

    public void UpdateHealth()
    {

        //for (int i = 0; i + 1 <= healthController.healthPoints; i++)
        //{
        //    healthSprites[i].color = Color.cyan;
        //}

        //for (int i = healthController.maxHealth-1; i >= healthController.healthPoints; i--)
        //{
        //    healthSprites[i].color = Color.white;
        //}

        hasTakeDamage = true;
        healthSlider.value = healthController.healthPoints;
    }

    public void TakingDamage()
    {
        if (hasTakeDamage == true)
        {
            StartCoroutine(FeedbackSecondSlider());
        }
    }

    IEnumerator FeedbackSecondSlider()
    {

        yield return new WaitForSeconds(0.5f);

        if (secondHealthSlider.value > healthSlider.value)
        {
            secondHealthSlider.value -= Time.deltaTime * 0.8f;
        }

        else if (secondHealthSlider.value > healthSlider.value && isBoss)
        {
            secondHealthSlider.value -= Time.deltaTime * 4f;
        }

        else if (secondHealthSlider.value <= healthSlider.value)
        {
                secondHealthSlider.value = healthSlider.value;
                hasTakeDamage = false;
        }

        yield return null;
    }

}
