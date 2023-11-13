using System;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    #region Facade

    public DashUIManager GetDashUIManager => _dashUIManager;
    [SerializeField] private DashUIManager _dashUIManager;
    public SlamUIManager GetSlamUIManager => _slamUIManager;
    [SerializeField] private SlamUIManager _slamUIManager;

    public HealthUIManager GetHealthUIManager => _healthUIManager;
    [SerializeField] private HealthUIManager _healthUIManager;

    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _map;

    public BulletsUIManager GetBulletsUIManager => _bulletsUIManager;
    [SerializeField] private BulletsUIManager _bulletsUIManager;
    #endregion

    public static UIManager Instance;
    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    public void OpenInventory()
    {
        _inventory.SetActive(!_inventory.activeInHierarchy);
        _map.SetActive(false);
    }

    public void OpenMap()
    {
        _map.SetActive(!_map.activeInHierarchy);
        _inventory.SetActive(false);
    }
}

[Serializable]
public class DashUIManager
{
    [SerializeField] private GameObject _dashIcon;
    [SerializeField] private Image _cooldownImage;
    
    public void UpdateCooldown(float currentTime,float maxTime)
    {
        _cooldownImage.fillAmount =currentTime/maxTime ;
    }

    public void UnlockAbility()
    {
        _dashIcon.SetActive(true);;
    }
}

[Serializable]
public class SlamUIManager
{
    [SerializeField] private GameObject _slamIcon;
    [SerializeField] private Image _cooldownImage;

    public void UpdateCooldown(float currentTime, float maxTime)
    {
        _cooldownImage.fillAmount = currentTime / maxTime;
    }

    public void UnlockAbility()
    {
        _slamIcon.SetActive(true); ;
    }
}

[Serializable]
public class HealthUIManager
{
    [SerializeField] private Slider _healthSlider;

    public void SetHealth(float currentHealth, float maxHealth)
    {
        _healthSlider.value = currentHealth / maxHealth;
    }
}


[Serializable]
public class BulletsUIManager
{
    [SerializeField] private GameObject _gunPanel;
    [SerializeField] private Transform _bulletsParent;
    [SerializeField] private GameObject[] _ammo;
    public void UnlockGun()
    {
        _gunPanel.SetActive(true);
        
        _ammo = new GameObject[_bulletsParent.childCount];
        
        for (int i = 0; i < _ammo.Length; i++)
        {
            _ammo[i] = _bulletsParent.GetChild(i).gameObject;
        }
        Debug.Log("ammos " + _ammo.Length);
    }
    
    public void UpdateBullets(int currentAmmo)
    {
        for (int i = 0; i < _ammo.Length; i++)
        {
            if (i < currentAmmo)
            {
                _ammo[i].SetActive(true);
            }
            else
            {
                _ammo[i].SetActive(false);
            }
        }
    }
}