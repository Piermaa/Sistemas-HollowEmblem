using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class Slot
{
    public int positionX, positionY, amount;
    public Item item;
    public Image image;
    public GameObject slotGameObject;
    public TextMeshProUGUI tMPro;
    public UnityEvent itemEvent;
    public Button button;
    public PopUp slotPopUp;
    public MouseDetector mouseDetector;
   
    public Slot(Item i, int pX, int pY, int a, Image im, GameObject go,TextMeshProUGUI t,Button b)
    {
        this.item = i;
        this.positionX = pX;
        this.positionY = pY;
        this.amount = a;
        this.image = im;
        this.button = b;
        this.slotGameObject=go;
        this.tMPro = t; 
    }
}

public class PlayerInventory : MonoBehaviour
{
    [Header("Objects")]
    public Slot[,] slots;
    public static PlayerInventory Instance;
    private PlayerCombat combat;
    private ItemManager itemManager;

    [Header("Ints")]
    public int rowSize = 4;
    public int colSize = 2;
    public int auxAmount; // REQUERIDA COMO GLOBAL PARA HACER CALCULOS DENTRO DE VARIAS FUNCIONES


    public GameObject[] rows;

    private void Awake()
    {
    
        Instance = this;//SINGLETON
    }
    void Start()
    {
        itemManager = ItemManager.Instance;
        combat = GetComponent<PlayerCombat>();
        slots = new Slot[rowSize, colSize];

        // Y ES EL VERTICAL, MAXIMO 3
        // X HORIZONTAL, MAXIMO 6
        // ARRAY[X,Y]
        for (int y = 0; y < slots.GetLength(1); y++)
        {
            var butRow = rows[y].GetComponentsInChildren<Button>(); // LOS "SLOTS" SE GUARDAN DE A FILAS EN LA JERARQUIA POR LO QUE SE PUEDE ACCEDER A ELLOS ASI

            for (int x = 0; x < slots.GetLength(0); x++)
            {
                //SE CREAN INSTANCIAS DE LA CLASE SLOT Y SE ASIGNAN A CADA LUGAR DEL INVENTARIO
                slots[x, y] = new Slot(null, x, y, 0, null, butRow[x].gameObject, null, butRow[x]); //COMO CADA SLOT ES UN BOTON CON SU RESPECTIVO GAME OBJECT LO USAMOS PARA ACCEDER A LO DEMAS
                slots[x, y].tMPro = slots[x, y].slotGameObject.GetComponentInChildren<TextMeshProUGUI>();
                slots[x, y].tMPro.text = " "; // TEXTO NULO PARA QUE NO SE VEA NADA, QUEDA MEJOR QUE PONER 0
                var buttonImage = slots[x, y].slotGameObject.GetComponent<Image>();      
                var images =slots[x, y].slotGameObject.GetComponentsInChildren<Image>();
                slots[x, y].image = images[1];
                slots[x, y].image.enabled = false; // SE DESACTIVA LA IMAGEN DEL ITEM, NO LA DEL BOTON
            }
        }
    }

    /// <summary>
    /// Se añade cierta cantidad de items al inventario, para ello buscará un lugar disponible en el inventario
    /// </summary>
    /// <param name="itemToAdd">Tipo de item a agregar</param>
    /// <param name="amountToAdd">Cantidad a agregar</param>
    public void AddItem(Item itemToAdd, int amountToAdd)
    {
        Slot actualSlot = SearchSlot(itemToAdd, amountToAdd);

        if (actualSlot != null && auxAmount != 0)
        {
            actualSlot.itemEvent = itemToAdd.itemEvent;
            actualSlot.item = itemToAdd;
            actualSlot.amount += auxAmount;
            actualSlot.image.enabled = true;
            actualSlot.image.sprite = itemToAdd.sprite;
            actualSlot.tMPro.text = actualSlot.amount.ToString();

            //var popUpReference = actualSlot.button.gameObject.GetComponentsInChildren<PopUp>(); // SE ACCEDE AL
            //Debug.Log("POPUPS: " + popUpReference.Length);
            if (actualSlot.slotPopUp == null) // SI EL SLOT NO TIENE UN POP UP ASIGNADO, SE CREA UNO
            {
                CreatePopUp(actualSlot);
            }
        }
    }

    /// <summary>
    /// Variante por si ya se tiene el slot que se quiere afectar
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    /// <param name="actualSlot"></param>
    public void AddItem(Item itemToAdd, int amountToAdd, Slot actualSlot)
    {
        actualSlot.item = itemToAdd;
        actualSlot.amount += auxAmount;
        if (actualSlot.amount >= itemToAdd.maxStackeable)
        {
            actualSlot.amount = itemToAdd.maxStackeable;
        }
        actualSlot.image.enabled = true;
        actualSlot.image.sprite = itemToAdd.sprite;
        actualSlot.tMPro.text = actualSlot.amount.ToString();

        //var popUpReference = actualSlot.button.gameObject.GetComponentsInChildren<PopUp>();
        //Debug.Log("POPUPS: " +popUpReference.Length);
        if (actualSlot.slotPopUp == null)
        {
            CreatePopUp(actualSlot);
        }
    }
    /// <summary>
    /// Recorre el array de Slots hasta encontrar un lugar donde colocar el item que se quiere añadir
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    /// <returns></returns>
    public Slot SearchSlot(Item itemToAdd, int amountToAdd)
    {
        int restantAmount = amountToAdd;
        auxAmount = restantAmount; //AUX AMOUNT ES UTILIZADO COMO CANTIDAD DE OBJETOS A AGREGAR EN CASO DE QUE EL MONTO A AGREGAR SEA MODIFICADO MIENTRAS SE BUSCA UN SLOT QUE PUEDAN OCUPAR LOS ITEMS
        print("auxAmount: " + auxAmount);

        for (int i = 0; i < 3; i++) // REPITE TRES VECES, MEDIDA DE SEGURIDAD EN CASO DE QUE NO HAYA LUGAR DONDE METER LOS ITEMS, MEJORA DE UN WHILE
        {
            if (restantAmount > 0) // SI TODAVIA QUEDA MONTO QUE AGREGAR:
            {
                for (int y = 0; y < slots.GetLength(1); y++) // LOOPEA EN LAS COLUMNAS
                {
                    for (int x = 0; x < slots.GetLength(0); x++)
                    {
                        Debug.Log(auxAmount);
                        if (slots[x, y].item == null) // SI EL SLOT ESTÀ VACIO
                        {
                            if (restantAmount < itemToAdd.maxStackeable)
                            {
                                return slots[x, y]; //SI EL SLOT ESTÁ VACÍO Y EL MONTO A AGREGAR ES MENOR AL MAXIMO STACKEABLE POR SLOT
                            }
                            else // SI EL SLOT ESTÁ VACÍO, PERO SE ESTÁ POR AGREGAR UNA CANTIDAD MAYOR A LA MAXIMA ACUMULABLE POR ESE ITEM
                            {
                                restantAmount -= itemToAdd.maxStackeable;
                                auxAmount = restantAmount;
                                FillSlot(slots[x, y], itemToAdd);
                            }
                        }
                        if (slots[x, y].item == itemToAdd) //SI ES EL MISMO TIPO DE ITEM EL DEL SLOT
                        {
                            if (!(slots[x, y].item.maxStackeable == slots[x, y].amount)) //SI EL SLOT NO ESTÁ LLENO:
                            {
                                int available = slots[x, y].item.maxStackeable - slots[x, y].amount;

                                if (restantAmount < available) // SI EL MONTO A AGREGAR NO HACE SUPERAR EL MÁXIMO STACKEABLE, CREO QUE PUEDE NO SER NECESARIO !!
                                {
                                    return slots[x, y];  //Devuelve este slot, porque se puede agregar aunque ocupando 
                                }
                                else // SI NO HAY TANTOS ESPACIOS DISPONIBLES COMO OBJETOS A AGREGAR
                                {
                                    restantAmount -= available;
                                    auxAmount = restantAmount;
                                    FillSlot(slots[x, y], itemToAdd);
                                }
                            }
                        }

                    }// X foR

                }// Y for

            }// if that check remaining items to add

        }//firewall for
        return null;
    }
    /// <summary>
    /// Vaciado de un slot, no solo se pone en 0 su cantidad, sino que se le quita toda la información propia del objeto que portaba
    /// </summary>
    /// <param name="slot"></param>
    public void EmptySlot(Slot slot)
    {
        DepleteSlot(slot);
        //var popUpReference = slot.button.gameObject.GetComponentsInChildren<PopUp>();
        Destroy(slot.slotPopUp.gameObject);

        //Debug.Log(slots[slot.positionX, slot.positionY]);
        Debug.Log(slot.positionX +","+ slot.positionY);
        if (slot.positionX < slots.GetLength(0) - 1)
        {
            Debug.Log(slot.positionX + 1 + "," + slot.positionY);
            MoveItem(slots[slot.positionX + 1, slot.positionY], slots[slot.positionX, slot.positionY]);
            Debug.Log(slots[slot.positionX + 1, slot.positionY]);
        }
        else if(slot.positionY < slots.GetLength(1) - 1)
        {
            Debug.Log(0 + "," + slot.positionY+1);
            MoveItem(slots[0, slot.positionY+1], slots[slot.positionX, slot.positionY]);
            //Debug.Log(slots[slot.positionX + 1, slot.positionY]);
        }
           
    }
    /// <summary>
    /// MUEVE EL CONTENIDO DE UN SLOT A OTRO Y VACIA EL PRIMERO
    /// </summary>
    /// <param name="slotOrigin">EL SLOT DESDE EL QUE SE MUEVEN LOS ITEMS</param>
    /// <param name="slotDestiny">EL SLOT AL QUE SE MOVERAN LOS ITEMS</param>
    void MoveItem(Slot slotOrigin, Slot slotDestiny)
    {
        if (slotOrigin.item!=null)
        {
            slotDestiny.amount = slotOrigin.amount;
            slotDestiny.itemEvent = slotOrigin.itemEvent;
            slotDestiny.tMPro.text = slotOrigin.tMPro.text;
            slotDestiny.item = slotOrigin.item;
            slotDestiny.image.sprite = slotOrigin.image.sprite;
            slotDestiny.image.enabled = slotOrigin.image.enabled;
            CreatePopUp(slotDestiny);
            EmptySlot(slotOrigin);
        }
       
    }
    void DepleteSlot(Slot slot)
    {
        slot.amount = 0;
        slot.itemEvent = null;
        slot.tMPro.text = "";
        slot.item = null;
        slot.image.sprite = null;
        slot.image.enabled = false;
        slot.mouseDetector.popUp = null;
    }
    /// <summary>
    /// Llena un slot dado por un item dado
    /// </summary>
    /// <param name="slot">Slot a llenar</param>
    /// <param name="itemType">Tipo de item que se colocará en el slot</param>
    public void FillSlot(Slot slot, Item itemType)
    {
        slot.itemEvent = itemType.itemEvent;
        slot.item = itemType;
        slot.amount = itemType.maxStackeable;
        slot.tMPro.text = slot.amount.ToString();
        slot.image.enabled = true;
        slot.image.sprite = itemType.sprite;
        var popUpReference = slot.button.gameObject.GetComponentsInChildren<PopUp>();
        //Debug.Log("POPUPS: " + popUpReference.Length);
        if (slot.slotPopUp==null)
        {
            CreatePopUp(slot);
        }
    }
    /// <summary>
    /// Creacion de Pop ups para utilizar los items del inventario
    /// </summary>
    /// <param name="slot"></param>
    public void CreatePopUp(Slot slot)
    {
        slot.item.popUp = Instantiate(itemManager.popUpPrefabs, slot.slotGameObject.transform); // Se crea un GameObject y se lo hace hijo del Slot
        slot.item.popUp.TryGetComponent<PopUp>(out var popUpRef); // Se obtiene una referencia del Objeto de la clase Pop Up del game object instanciado
        slot.item.popUp.gameObject.SetActive(false); // Se desactiva porque solo se debe ver al clickear el boton del slot
        popUpRef.slot = slot; // Se asigna el slot del pop up
        slot.slotPopUp = popUpRef; // Se le asigna una referencia al objeto popup del slot

        slot.button.onClick.AddListener(popUpRef.ActivatePopUp); // Se asigna el evento del boton del slot

        slot.mouseDetector =slot.slotGameObject.AddComponent<MouseDetector>();
        slot.mouseDetector.popUp = popUpRef;

        
        //Se asigna el uso del boton Use de los pop ups, depende del nombre del item añadido.
        switch(slot.item.name)
        {

            case "Heal":
          
                slot.slotPopUp.useButton.onClick.AddListener(delegate { ConsumeItem(slot); });
                slot.slotPopUp.useButton.onClick.AddListener(itemManager.UseHeal);
                break;

            case "Ammo":
                //slot.slotPopUp.useButton.onClick += combat.Reload();
                slot.slotPopUp.useButton.onClick.AddListener(combat.Reload);
                break;

        }
    }
   
     /// <summary>
     /// Se consumen items del slot y se actualiza su informacion
     /// </summary>
     /// <param name="slot">Slot del que se consumen objetos, cantidad de consumo es dada por el item</param>
    public void ConsumeItem(Slot slot)
    {
        slot.amount -= slot.item.usedPerEvent;
        slot.tMPro.text = slot.amount.ToString();

        if (slot.amount <= 0)
        {
            EmptySlot(slot);
        }
    }
    /// <summary>
    /// A traves de comparaciones, busca el slot que menos balas tiene.
    /// </summary>
    /// <returns></returns>
    public Slot SearchAmmo()
    {
        Slot lessAmmoSlot=null;// = slots[0,0];
        for (int y = 0; y < slots.GetLength(1); y++)
        {
            for (int x = 0; x < slots.GetLength(0); x++)
            {
                if (slots[x,y].item != null) //Si tiene algun item:
                {
                    if (slots[x, y].item.name == "Ammo") //Si el nombre del item es el del item de municion
                    {
                        if (lessAmmoSlot == null) // Si no se habia encontrado un slot que sea el de menor cantidad de balas, entonces es el actual
                        {
                            lessAmmoSlot = slots[x, y];
                        }
                        if (lessAmmoSlot.amount >= slots[x, y].amount) // Si el slot actual tiene menos balas que el que antes era el que menos tenia, ahora este es el que menos tiene
                            //           mayor o igual para que sea el ultimo encontrado si es que tienen varios la misma cantidad
                        {
                            lessAmmoSlot = slots[x, y];
                        }
                    }

                }

            }
        }
        return lessAmmoSlot;
    }
    /// <summary>
    /// Agrega municion al cargador del Player, buscará hasta encontrar mas municion si se vacia el slot
    /// </summary>
    public int GetAmmoFromInventory(bool justChecking)
    {
        int spaceAvailableOnClip=0; 
        Slot newSlot;
        
        do
        {
            spaceAvailableOnClip = combat.maxAmmo - combat.currentAmmo; // Se obtiene el espacio en el cargador
            //print(spaceAvailableOnClip);
            newSlot = SearchAmmo(); //S busca el slot con menos balas
            if (newSlot != null) // Si se encuentra:
            {
                if (spaceAvailableOnClip > newSlot.amount) //Si las balas del slot son menos que las requeridas para llenar el cargador:
                {
                    int auxAmmo = newSlot.amount;
                    //combat.currentAmmo += newSlot.amount; //Sumar todas las balas al cargador
                    if (!justChecking)
                    {
                        EmptySlot(newSlot); // Y vaciar el slot
                    }
                    Debug.Log("SE RETURNEA newslotSmount: " + newSlot.amount);
                    return auxAmmo;
                }
                else
                {
                    //combat.currentAmmo += spaceAvailableOnClip; // Si alcanza con las balas del slot para llenar el cargador, llenarlo
                    if (!justChecking)
                    {
                        newSlot.amount -= spaceAvailableOnClip; // Y actualizar el monto del slot
                        newSlot.tMPro.text = newSlot.amount.ToString();
                        if ((newSlot != null) && newSlot.amount <= 0)
                        {
                            EmptySlot(newSlot);
                        }
                    }
                    Debug.Log("SE RETURNEA spaceAvailableOnClip: " + spaceAvailableOnClip);
                    return spaceAvailableOnClip;
                }
            }

            //Si quedaban 0 balas en el slot por alguna razon, vaciarlo
        


        } while (spaceAvailableOnClip > 0 && newSlot != null); // El slot no era null porque se habia encontrado en la busqueda anterior

        return 0;
       
    }
}//class





