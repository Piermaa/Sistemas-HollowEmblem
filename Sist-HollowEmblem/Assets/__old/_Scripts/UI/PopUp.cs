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
        
        
        DeactivatePanel();
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

    public void Use()
    {
        switch (_slotOwner.ItemType)
        {
            case ItemTypes.HEAL:
                UseHeal();
                break;
            case ItemTypes.AMMO:
                UseAmmo();
                break;
        }
        
    }

    private void UseHeal()
    {
        ActionsManager.InvokeAction(ItemConstants.USE_HEAL);
        InventoryActionsManager.InvokeAction(ItemConstants.USE_HEAL, _slotOwner);
    }

    private void UseAmmo()
    {
        InventoryActionsManager.InvokeAction(ItemConstants.USE_AMMO, _slotOwner);
    }

    public void Discard() // SE añade el listener desde el inspector **
    {
        InventoryActionsManager.InvokeAction(ItemConstants.DISCARD, _slotOwner);
        if (_slotOwner.IsEmpty())
        {
            DeactivatePanel();
        }
    }
}
