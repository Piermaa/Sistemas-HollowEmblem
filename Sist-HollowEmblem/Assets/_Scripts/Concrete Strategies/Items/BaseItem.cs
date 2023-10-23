using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseItem : Pickupable, IItem
{
    #region IItem Properties

    public ItemTypes ItemType => _itemType;
    public int Amount => _amount;
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
        player?.GetComponent<PlayerInventory>().AddItem(this, _amount);
        // _audioSource.Play();
        Destroy(gameObject);
    }

    #endregion

    #region IItem Methods

    public virtual void Use()
    {
        //ActionsManager.InvokeAction(ItemConstants.USE_AMMO);
    }

    #endregion

    #region IPickupable Methods

    public override void OnPickup(Collider2D player)
    {
        AddToInventory(player);
        base.OnPickup(player);
    }

    #endregion
}
