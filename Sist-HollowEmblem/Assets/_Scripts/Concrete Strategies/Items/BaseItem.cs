using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseItem : Pickupable, IItem
{
    #region IItem Properties

    public Slot ItemSlot => _itemSlot;
    public ItemTypes ItemType => _itemType;
    public int Amount
    {
        set => _amount = value;
        get => _amount;
    }
    public int MaxStackeable => _maxStackeable;
    public int UnitsPerUse => _unitsPerUse;
    public Sprite ItemSprite => _spriteRenderer.sprite;

    #endregion

    #region Class Properties

    #region Serialized Variables

    [SerializeField] private int _amount;
    [SerializeField] private int _maxStackeable;
    [SerializeField] private int _unitsPerUse;
    [SerializeField] private ItemTypes _itemType;

    #endregion
    private SpriteRenderer _spriteRenderer;
    private Slot _itemSlot;
    #endregion

    #region Monobehaviour Callbacks

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        //    _amount = Random.Range(1,_maxStackeable);
    }

    #endregion

    #region Class Methods

    public virtual void AddToInventory(Collider2D player)
    {
        _itemSlot = player?.GetComponent<PlayerInventory>().AddItem(this, _amount);
        if (_itemSlot!=null)
        {
            _itemSlot.onItemUse += UseItem;
            base.OnPickup(player);
        }
    }

    public void SetSlot(Slot newSlot)
    {
        _itemSlot = newSlot;
    }

    #endregion

    #region IItem Methods

    public virtual void UseItem()
    {
    }

    #endregion

    #region IPickupable Methods

    public override void OnPickup(Collider2D player)
    {
        AddToInventory(player);
    }

    #endregion
}
