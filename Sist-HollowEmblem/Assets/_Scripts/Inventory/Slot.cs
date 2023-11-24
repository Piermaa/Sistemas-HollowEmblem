using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot: MonoBehaviour
{
    #region Public Properties

    public PopUp SlotPopUp => _slotPopUp;
    public Vector2Int Position => new Vector2Int(_positionX, _positionY);
    public IItem Item => _item;
    public ItemTypes ItemType
    {
        get
        {
            if (_item!=null)
            {
                return _item.ItemType;
            }
            else
            {
                return ItemTypes.NONE;
            }
        }
    }
    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            _amountText.text = _amount.ToString();

            _amountText.enabled = _amount > 0;
        }
    }

    #endregion

    public Action onItemUse= () => { };
    
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Button _button;
    [SerializeField] private PopUp _slotPopUp;
    
    private IItem _item=null;
    private int _positionX, _positionY;
    private int _amount = 0;
    
    private void OnMouseEnter()
    {
        print("atroden");
        _slotPopUp.ActivatePanel();
    }

    private void OnMouseExit()
    {
        _slotPopUp.DeactivatePanel();
    }

    #region Public Methods
    public void SetSlot(int pX, int pY)
    {
        this._positionX = pX;
        this._positionY = pY;
    
        _slotPopUp.SetSlot(this);
    }
    public void AddItem(IItem itemToAdd, int amount)
    {
        _item = itemToAdd;
        Amount += amount;
        _image.enabled = true;
        _image.sprite = itemToAdd.ItemSprite;
    }
    public void Deplete()
    {
        Amount = 0;
        _item = null;
        _image.sprite = null;
        _image.enabled = false;
        // legacySlot.mouseDetector.popUp = null;
    }
    public void Fill()
    {
        Amount = _item.MaxStackeable;
    }

    public void RemoveItems(int amountToRemove)
    {
        Amount -= amountToRemove;
    }

    #endregion

    #region Functions

    #region Bool

    public bool IsEmpty()
    {
        return _amount == 0 || _item==null;
    }

    public bool CompareItems(IItem itemToCompare)
    {
        return _item.ItemType == itemToCompare.ItemType;
    }

    #endregion
  
    
    public int AmountToFill()
    {
        return  _item.MaxStackeable - _amount;
    }
     
    #endregion
}
