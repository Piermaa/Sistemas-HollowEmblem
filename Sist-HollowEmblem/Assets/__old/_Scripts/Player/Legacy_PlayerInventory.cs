using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class Legacy_Slot
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
   
    public Legacy_Slot(Item i, int pX, int pY, int a, Image im, GameObject go,TextMeshProUGUI t,Button b)
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

public class Legacy_PlayerInventory : MonoBehaviour
{
    [Header("Objects")]
    public Legacy_Slot[,] slots;
    public static Legacy_PlayerInventory Instance;
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
        slots = new Legacy_Slot[rowSize, colSize];

        // Y ES EL VERTICAL, MAXIMO 3
        // X HORIZONTAL, MAXIMO 6
        // ARRAY[X,Y]
        for (int y = 0; y < slots.GetLength(1); y++)
        {
            var butRow = rows[y].GetComponentsInChildren<Button>(); // LOS "SLOTS" SE GUARDAN DE A FILAS EN LA JERARQUIA POR LO QUE SE PUEDE ACCEDER A ELLOS ASI

            for (int x = 0; x < slots.GetLength(0); x++)
            {
                //SE CREAN INSTANCIAS DE LA CLASE SLOT Y SE ASIGNAN A CADA LUGAR DEL INVENTARIO
                slots[x, y] = new Legacy_Slot(null, x, y, 0, null, butRow[x].gameObject, null, butRow[x]); //COMO CADA SLOT ES UN BOTON CON SU RESPECTIVO GAME OBJECT LO USAMOS PARA ACCEDER A LO DEMAS
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
    /// Se a�ade cierta cantidad de items al inventario, para ello buscar� un lugar disponible en el inventario
    /// </summary>
    /// <param name="itemToAdd">Tipo de item a agregar</param>
    /// <param name="amountToAdd">Cantidad a agregar</param>
    public void AddItem(Item itemToAdd, int amountToAdd)
    {
        Legacy_Slot actualLegacySlot = SearchSlot(itemToAdd, amountToAdd);

        if (actualLegacySlot != null && auxAmount != 0)
        {
            actualLegacySlot.itemEvent = itemToAdd.itemEvent;
            actualLegacySlot.item = itemToAdd;
            actualLegacySlot.amount += auxAmount;
            actualLegacySlot.image.enabled = true;
            actualLegacySlot.image.sprite = itemToAdd.sprite;
            actualLegacySlot.tMPro.text = actualLegacySlot.amount.ToString();

            //var popUpReference = actualSlot.button.gameObject.GetComponentsInChildren<PopUp>(); // SE ACCEDE AL
            //Debug.Log("POPUPS: " + popUpReference.Length);
            if (actualLegacySlot.slotPopUp == null) // SI EL SLOT NO TIENE UN POP UP ASIGNADO, SE CREA UNO
            {
                CreatePopUp(actualLegacySlot);
            }
        }
    }

    /// <summary>
    /// Variante por si ya se tiene el slot que se quiere afectar
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    /// <param name="actualLegacySlot"></param>
    public void AddItem(Item itemToAdd, int amountToAdd, Legacy_Slot actualLegacySlot)
    {
        actualLegacySlot.item = itemToAdd;
        actualLegacySlot.amount += auxAmount;
        if (actualLegacySlot.amount >= itemToAdd.maxStackeable)
        {
            actualLegacySlot.amount = itemToAdd.maxStackeable;
        }
        actualLegacySlot.image.enabled = true;
        actualLegacySlot.image.sprite = itemToAdd.sprite;
        actualLegacySlot.tMPro.text = actualLegacySlot.amount.ToString();

        //var popUpReference = actualSlot.button.gameObject.GetComponentsInChildren<PopUp>();
        //Debug.Log("POPUPS: " +popUpReference.Length);
        if (actualLegacySlot.slotPopUp == null)
        {
            CreatePopUp(actualLegacySlot);
        }
    }
    /// <summary>
    /// Recorre el array de Slots hasta encontrar un lugar donde colocar el item que se quiere a�adir
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    /// <returns></returns>
    public Legacy_Slot SearchSlot(Item itemToAdd, int amountToAdd)
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
                        if (slots[x, y].item == null) // SI EL SLOT EST� VACIO
                        {
                            if (restantAmount < itemToAdd.maxStackeable)
                            {
                                return slots[x, y]; //SI EL SLOT EST� VAC�O Y EL MONTO A AGREGAR ES MENOR AL MAXIMO STACKEABLE POR SLOT
                            }
                            else // SI EL SLOT EST� VAC�O, PERO SE EST� POR AGREGAR UNA CANTIDAD MAYOR A LA MAXIMA ACUMULABLE POR ESE ITEM
                            {
                                restantAmount -= itemToAdd.maxStackeable;
                                auxAmount = restantAmount;
                                FillSlot(slots[x, y], itemToAdd);
                            }
                        }
                        if (slots[x, y].item == itemToAdd) //SI ES EL MISMO TIPO DE ITEM EL DEL SLOT
                        {
                            if (!(slots[x, y].item.maxStackeable == slots[x, y].amount)) //SI EL SLOT NO EST� LLENO:
                            {
                                int available = slots[x, y].item.maxStackeable - slots[x, y].amount;

                                if (restantAmount < available) // SI EL MONTO A AGREGAR NO HACE SUPERAR EL M�XIMO STACKEABLE, CREO QUE PUEDE NO SER NECESARIO !!
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
    /// Vaciado de un slot, no solo se pone en 0 su cantidad, sino que se le quita toda la informaci�n propia del objeto que portaba
    /// </summary>
    /// <param name="legacySlot"></param>
    public void EmptySlot(Legacy_Slot legacySlot)
    {
        DepleteSlot(legacySlot);
        //var popUpReference = slot.button.gameObject.GetComponentsInChildren<PopUp>();
        Destroy(legacySlot.slotPopUp.gameObject);

        //Debug.Log(slots[slot.positionX, slot.positionY]);
        Debug.Log(legacySlot.positionX +","+ legacySlot.positionY);
        if (legacySlot.positionX < slots.GetLength(0) - 1)
        {
            Debug.Log(legacySlot.positionX + 1 + "," + legacySlot.positionY);
            MoveItem(slots[legacySlot.positionX + 1, legacySlot.positionY], slots[legacySlot.positionX, legacySlot.positionY]);
            Debug.Log(slots[legacySlot.positionX + 1, legacySlot.positionY]);
        }
        else if(legacySlot.positionY < slots.GetLength(1) - 1)
        {
            Debug.Log(0 + "," + legacySlot.positionY+1);
            MoveItem(slots[0, legacySlot.positionY+1], slots[legacySlot.positionX, legacySlot.positionY]);
            //Debug.Log(slots[slot.positionX + 1, slot.positionY]);
        }
           
    }
    /// <summary>
    /// MUEVE EL CONTENIDO DE UN SLOT A OTRO Y VACIA EL PRIMERO
    /// </summary>
    /// <param name="legacySlotOrigin">EL SLOT DESDE EL QUE SE MUEVEN LOS ITEMS</param>
    /// <param name="legacySlotDestiny">EL SLOT AL QUE SE MOVERAN LOS ITEMS</param>
    void MoveItem(Legacy_Slot legacySlotOrigin, Legacy_Slot legacySlotDestiny)
    {
        if (legacySlotOrigin.item!=null)
        {
            legacySlotDestiny.amount = legacySlotOrigin.amount;
            legacySlotDestiny.itemEvent = legacySlotOrigin.itemEvent;
            legacySlotDestiny.tMPro.text = legacySlotOrigin.tMPro.text;
            legacySlotDestiny.item = legacySlotOrigin.item;
            legacySlotDestiny.image.sprite = legacySlotOrigin.image.sprite;
            legacySlotDestiny.image.enabled = legacySlotOrigin.image.enabled;
            CreatePopUp(legacySlotDestiny);
            EmptySlot(legacySlotOrigin);
        }
       
    }
    void DepleteSlot(Legacy_Slot legacySlot)
    {
        legacySlot.amount = 0;
        legacySlot.itemEvent = null;
        legacySlot.tMPro.text = "";
        legacySlot.item = null;
        legacySlot.image.sprite = null;
        legacySlot.image.enabled = false;
        legacySlot.mouseDetector.popUp = null;
    }
    /// <summary>
    /// Llena un slot dado por un item dado
    /// </summary>
    /// <param name="legacySlot">Slot a llenar</param>
    /// <param name="itemType">Tipo de item que se colocar� en el slot</param>
    public void FillSlot(Legacy_Slot legacySlot, Item itemType)
    {
        legacySlot.itemEvent = itemType.itemEvent;
        legacySlot.item = itemType;
        legacySlot.amount = itemType.maxStackeable;
        legacySlot.tMPro.text = legacySlot.amount.ToString();
        legacySlot.image.enabled = true;
        legacySlot.image.sprite = itemType.sprite;
        var popUpReference = legacySlot.button.gameObject.GetComponentsInChildren<PopUp>();
        //Debug.Log("POPUPS: " + popUpReference.Length);
        if (legacySlot.slotPopUp==null)
        {
            CreatePopUp(legacySlot);
        }
    }
    /// <summary>
    /// Creacion de Pop ups para utilizar los items del inventario
    /// </summary>
    /// <param name="legacySlot"></param>
    public void CreatePopUp(Legacy_Slot legacySlot)
    {
        legacySlot.item.popUp = Instantiate(itemManager.popUpPrefabs, legacySlot.slotGameObject.transform); // Se crea un GameObject y se lo hace hijo del Slot
        legacySlot.item.popUp.TryGetComponent<PopUp>(out var popUpRef); // Se obtiene una referencia del Objeto de la clase Pop Up del game object instanciado
        legacySlot.item.popUp.gameObject.SetActive(false); // Se desactiva porque solo se debe ver al clickear el boton del slot
       // popUpRef.LegacySlot = legacySlot; // Se asigna el slot del pop up
        legacySlot.slotPopUp = popUpRef; // Se le asigna una referencia al objeto popup del slot

     //   legacySlot.button.onClick.AddListener(popUpRef.ActivatePopUp); // Se asigna el evento del boton del slot

        legacySlot.mouseDetector =legacySlot.slotGameObject.AddComponent<MouseDetector>();
        legacySlot.mouseDetector.popUp = popUpRef;

        
        //Se asigna el uso del boton Use de los pop ups, depende del nombre del item a�adido.
        switch(legacySlot.item.name)
        {

            case "Heal":
          
 //               legacySlot.slotPopUp.useButton.onClick.AddListener(delegate { ConsumeItem(legacySlot); });
 //               legacySlot.slotPopUp.useButton.onClick.AddListener(itemManager.UseHeal);
                break;

            case "Ammo":
                //slot.slotPopUp.useButton.onClick += combat.Reload();
 //               legacySlot.slotPopUp.useButton.onClick.AddListener(combat.Reload);
                break;

        }
    }
   
     /// <summary>
     /// Se consumen items del slot y se actualiza su informacion
     /// </summary>
     /// <param name="legacySlot">Slot del que se consumen objetos, cantidad de consumo es dada por el item</param>
    public void ConsumeItem(Legacy_Slot legacySlot)
    {
        legacySlot.amount -= legacySlot.item.usedPerEvent;
        legacySlot.tMPro.text = legacySlot.amount.ToString();

        if (legacySlot.amount <= 0)
        {
            EmptySlot(legacySlot);
        }
    }
    /// <summary>
    /// A traves de comparaciones, busca el slot que menos balas tiene.
    /// </summary>
    /// <returns></returns>
    public Legacy_Slot SearchAmmo()
    {
        Legacy_Slot lessAmmoLegacySlot=null;// = slots[0,0];
        for (int y = 0; y < slots.GetLength(1); y++)
        {
            for (int x = 0; x < slots.GetLength(0); x++)
            {
                if (slots[x,y].item != null) //Si tiene algun item:
                {
                    if (slots[x, y].item.name == "Ammo") //Si el nombre del item es el del item de municion
                    {
                        if (lessAmmoLegacySlot == null) // Si no se habia encontrado un slot que sea el de menor cantidad de balas, entonces es el actual
                        {
                            lessAmmoLegacySlot = slots[x, y];
                        }
                        if (lessAmmoLegacySlot.amount >= slots[x, y].amount) // Si el slot actual tiene menos balas que el que antes era el que menos tenia, ahora este es el que menos tiene
                            //           mayor o igual para que sea el ultimo encontrado si es que tienen varios la misma cantidad
                        {
                            lessAmmoLegacySlot = slots[x, y];
                        }
                    }

                }

            }
        }
        return lessAmmoLegacySlot;
    }
    /// <summary>
    /// Agrega municion al cargador del Player, buscar� hasta encontrar mas municion si se vacia el slot
    /// </summary>
    public int GetAmmoFromInventory(bool justChecking)
    {
        int spaceAvailableOnClip=0; 
        Legacy_Slot newLegacySlot;
        
        do
        {
            spaceAvailableOnClip = combat.maxAmmo - combat.currentAmmo; // Se obtiene el espacio en el cargador
            //print(spaceAvailableOnClip);
            newLegacySlot = SearchAmmo(); //S busca el slot con menos balas
            if (newLegacySlot != null) // Si se encuentra:
            {
                if (spaceAvailableOnClip > newLegacySlot.amount) //Si las balas del slot son menos que las requeridas para llenar el cargador:
                {
                    int auxAmmo = newLegacySlot.amount;
                    //combat.currentAmmo += newSlot.amount; //Sumar todas las balas al cargador
                    if (!justChecking)
                    {
                        EmptySlot(newLegacySlot); // Y vaciar el slot
                    }
                    Debug.Log("SE RETURNEA newslotSmount: " + newLegacySlot.amount);
                    return auxAmmo;
                }
                else
                {
                    //combat.currentAmmo += spaceAvailableOnClip; // Si alcanza con las balas del slot para llenar el cargador, llenarlo
                    if (!justChecking)
                    {
                        newLegacySlot.amount -= spaceAvailableOnClip; // Y actualizar el monto del slot
                        newLegacySlot.tMPro.text = newLegacySlot.amount.ToString();
                        if ((newLegacySlot != null) && newLegacySlot.amount <= 0)
                        {
                            EmptySlot(newLegacySlot);
                        }
                    }
                    Debug.Log("SE RETURNEA spaceAvailableOnClip: " + spaceAvailableOnClip);
                    return spaceAvailableOnClip;
                }
            }

            //Si quedaban 0 balas en el slot por alguna razon, vaciarlo
        


        } while (spaceAvailableOnClip > 0 && newLegacySlot != null); // El slot no era null porque se habia encontrado en la busqueda anterior

        return 0;
       
    }
}//class





