using System;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    #region Facade

    public DashUIManager GetDashUIManager => _dashUIManager;
    [SerializeField] private DashUIManager _dashUIManager;

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
