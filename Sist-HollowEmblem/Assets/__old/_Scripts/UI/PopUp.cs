using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PopUp : MonoBehaviour
{
    public Slot slot;
    public Button discardButton;
    public Button useButton;
    public int xIndex;
    public int yIndex;
    public PlayerInventory inventory;

    public UnityEvent pointEvent;
    void Start()
    {
        inventory = PlayerInventory.Instance;
        pointEvent.AddListener(ActivatePopUp);
    }

    public void ActivatePopUp()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    public void ActivatePanel()
    {
        this.gameObject.SetActive(true);
    }
    public void DeactivatePanel()
    {
        this.gameObject.SetActive(false);
    }
    public void Discard() // SE añade el listener desde el inspector **
    {
        inventory.EmptySlot(slot);
        Destroy(this.gameObject);
    }

    public void UseItem()
    {
        slot.itemEvent.Invoke();
        slot.amount -= slot.item.usedPerEvent;
    }
  
}
