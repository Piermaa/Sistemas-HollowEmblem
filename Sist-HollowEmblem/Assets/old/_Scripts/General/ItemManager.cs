using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class Item
{
    public string name;
    public int maxStackeable;
    public Sprite sprite;
    public UnityEvent itemEvent;
    public GameObject popUp;
    public int usedPerEvent;
    public Item(string n, int mS, Sprite s, UnityEvent ev,GameObject popUp)
    {
        this.itemEvent = ev;
        this.name = n;
        this.maxStackeable = mS;
        this.sprite = s;
        this.popUp= popUp;
    }
}
public class ItemManager : MonoBehaviour
{
    public GameObject popUpPrefabs;
    #region Singleton
    public static ItemManager Instance;



    #endregion
    public List<Item> items=new List<Item>();

    public Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>();
    public PlayerInventory inventory;

    GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        foreach (Item it in items)
        {
            itemDictionary.Add(it.name, it);
        }
        //print("Dic count: " + itemDictionary.Count);
    }

    // Start is called before the first frame update
    void Start()
    {
        inventory = PlayerInventory.Instance;
        player = inventory.gameObject;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventory.AddItem(items[0], 3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventory.AddItem(items[1], 5);
        }
    }
    public void UseHeal()
    {
        int usedItems = 1;
        player.TryGetComponent<HealthController>(out var health);
        health.Heal(usedItems);
    }

    public void Craft()
    {
        // mentira 
    }
}
