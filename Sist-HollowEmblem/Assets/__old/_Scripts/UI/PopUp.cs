using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PopUp : MonoBehaviour
{
    [SerializeField] private Button _discardButton;
    [SerializeField] private Button _useButton;
    private int xIndex;
    private int yIndex;
    
    private Slot _slotOwner;
    public void SetSlot(Slot slot)
    {
        _slotOwner = slot;
        _discardButton.onClick.AddListener(_slotOwner.Deplete);
        
        DeactivatePanel();
    }

    public void ResetPopUp()
    {
        _useButton.onClick.RemoveAllListeners();
        _useButton.onClick.AddListener(_slotOwner.Item.Use);
    }

    public void ActivatePanel()
    {
        if (!_slotOwner.IsEmpty())
        {
            this.gameObject.SetActive(true);
        }
    }
    public void DeactivatePanel()
    {
        this.gameObject.SetActive(false);
    }
    public void Discard() // SE añade el listener desde el inspector **
    {
        DeactivatePanel();
    }
}
