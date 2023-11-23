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

    private int _maxHealth;
    private bool _hasTakenDamage = false;

    private void Awake()
    {
        _panel.SetActive(false);
    }

    public void InitializeBossUI(BossEnemy bossEnemy)
    {
        _maxHealth = bossEnemy.MaxHealth;
        _panel.SetActive(true);
        _bossNameText.text = bossEnemy.name;

        ActionsManager.SubscribeToAction(bossEnemy.gameObject.name + ActionConstants.DEATH, DisableBossUI);
        IntActionsManager.SubscribeToAction(bossEnemy.gameObject.name + ActionConstants.TAKE_DAMAGE,
            UpdateHealthSlider);
    }

    private void Update()
    {
        TakingDamage();
    }

    private void DisableBossUI()
    {
        ActionsManager.UnsubscribeToAction(  _bossNameText.text + ActionConstants.DEATH, DisableBossUI);
        IntActionsManager.UnsubscribeToAction(  _bossNameText.text + ActionConstants.TAKE_DAMAGE,
            UpdateHealthSlider);
        _panel.SetActive(false);
    }

    private void UpdateHealthSlider(int currentHealth)
    {
        _hasTakenDamage = true;
        _healthSlider.value = (float)currentHealth / _maxHealth;
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