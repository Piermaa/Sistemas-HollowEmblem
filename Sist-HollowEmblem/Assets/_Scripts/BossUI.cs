using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
   [SerializeField] private GameObject _panel;
   [SerializeField] private Slider _healthSlider;
   [SerializeField] private Slider _secondHealthSlider;
   [SerializeField] private Text _bossNameText;

   private bool _hasTakenDamage=false;
   private BossEnemy _bossEnemy;
   
   public void InitializeBossUI(BossEnemy bossEnemy)
   {
      _bossEnemy = bossEnemy;
      _panel.SetActive(true);
      _bossNameText.text = bossEnemy.name;
   }

   private void Update()
   {
      TakingDamage();
   }

   private void UpdateHealthSlider()
   {
      _hasTakenDamage = true;
      _healthSlider.value = _bossEnemy.CurrentHealth;
   }

   public void TakingDamage()
   {
      if (_hasTakenDamage == true)
      {
         StartCoroutine(FeedbackSecondSlider());
      }
   }

   IEnumerator FeedbackSecondSlider()
   {
      yield return new WaitForSeconds(0.5f);

      if (_secondHealthSlider.value > _healthSlider.value)
      {
         _secondHealthSlider.value -= Time.deltaTime * 0.8f;
      }

      else if (_secondHealthSlider.value > _healthSlider.value)
      {
         _secondHealthSlider.value -= Time.deltaTime * 4f;
      }

      else if (_secondHealthSlider.value <= _healthSlider.value)
      {
         _secondHealthSlider.value = _healthSlider.value;
         _hasTakenDamage = false;
      }

      yield return null;
   }
}
