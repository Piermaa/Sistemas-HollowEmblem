using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private AudioSource _pickUpSound;
    [SerializeField] private GameObject[] rows;
    [SerializeField] private int _rows = 2;
    [SerializeField] private int _columns = 4;
    private Slot[,] slots;

    private int _bulletsInInventory;

    public int BulletsInInventory => _bulletsInInventory;

    void Awake()
    {
        slots = new Slot[_rows, _columns];
        
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                //SE CREAN INSTANCIAS DE LA CLASE SLOT Y SE ASIGNAN A CADA LUGAR DEL INVENTARIO
                slots[i,j] = rows[i].transform.GetChild(j).GetComponent<Slot>(); 
                slots[i,j].SetSlot(i,j);
            }
        }
        
        InventoryActionsManager.RegisterAction(ItemConstants.USE_HEAL); //todo mover a q cada item tenga el propio
        InventoryActionsManager.SubscribeToAction(ItemConstants.USE_HEAL, ConsumeItem);
        
        InventoryActionsManager.RegisterAction(ItemConstants.DISCARD);
        InventoryActionsManager.SubscribeToAction(ItemConstants.DISCARD, EmptySlot);
    }

    /// <summary>
    /// Se a�ade cierta cantidad de items al inventario, para ello buscar� un lugar disponible en el inventario
    /// </summary>
    /// <param name="itemToAdd">Tipo de item a agregar</param>
    /// <param name="amountToAdd">Cantidad a agregar</param>
    public Slot AddItem(IItem itemToAdd, int amountToAdd)
    {
        Slot _currentSlot = SearchSlot(itemToAdd, ref amountToAdd);

        if (_currentSlot != null)
        {
            _currentSlot.AddItem(itemToAdd, amountToAdd);
            _pickUpSound.Play();
        }

        return _currentSlot;
    }
    
    public Slot SearchSlot(IItem itemToAdd,ref int amountToAdd)
    {
        if (amountToAdd > 0) // SI TODAVIA QUEDA MONTO QUE AGREGAR:
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    // Debug.Log(_auxAmount);
                    if (slots[i, j].IsEmpty()) // SI EL SLOT EST� VACIO
                    {
                        if (amountToAdd <= itemToAdd.MaxStackeable)
                        {
                            return slots
                                [i, j]; //SI EL SLOT EST� VAC�O Y EL MONTO A AGREGAR ES MENOR AL MAXIMO STACKEABLE POR SLOT
                        }
                        else // SI EL SLOT EST� VAC�O, PERO SE EST� POR AGREGAR UNA CANTIDAD MAYOR A LA MAXIMA ACUMULABLE POR ESE ITEM
                        {
                            amountToAdd -= itemToAdd.MaxStackeable;
                            FillSlot(slots[i, j]);
                            SearchSlot(itemToAdd, ref amountToAdd);
                        }
                    }

                    if (slots[i, j].CompareItems(itemToAdd)) //SI ES EL MISMO TIPO DE ITEM EL DEL SLOT
                    {
                        if (slots[i, j].AmountToFill() > 0) //SI EL SLOT NO EST� LLENO:
                        {
                            int available = slots[i, j].AmountToFill();

                            if (amountToAdd < available) // SI EL MONTO A AGREGAR NO HACE SUPERAR EL M�XIMO STACKEABLE, CREO QUE PUEDE NO SER NECESARIO !!
                            {
                                return slots[i, j]; //Devuelve este slot, porque se puede agregar aunque ocupando 
                            }
                            else // SI NO HAY TANTOS ESPACIOS DISPONIBLES COMO OBJETOS A AGREGAR
                            {
                                amountToAdd -= available;
                                FillSlot(slots[i, j]);
                                SearchSlot(itemToAdd, ref amountToAdd);
                            }
                        }
                    }
                } // X foR
            } // Y for
        } // if that check remaining items to add

        return null;
    }
    
    /// <summary>
    /// Vaciado de un slot, no solo se pone en 0 su cantidad, sino que se le quita toda la informaci�n propia del objeto que portaba
    /// </summary>
    /// <param name="slot"></param>
    public void EmptySlot(Slot slot)
    {
        slot.Deplete();
        Debug.Log($"Destination slot: {slot.Position.x}, {slot.Position.y}");
        if (slot.Position.y < _columns - 1)
        {
            Debug.Log($"Origin slot: {slot.Position.x}, {slot.Position.y+1}");

            MoveItem(slots[slot.Position.x, slot.Position.y+1], slot);
        }
        else if(slot.Position.x < _rows - 1)
        {
            Debug.Log($"Origin slot: {0}, {1}");    
        
            MoveItem(slots[slot.Position.x+1, 0], slot);
        }
    }
    /// <summary>
    /// MUEVE EL CONTENIDO DE UN SLOT A OTRO Y VACIA EL PRIMERO
    /// </summary>
    /// <param name="originSlot">EL SLOT DESDE EL QUE SE MUEVEN LOS ITEMS</param>
    /// <param name="destinationSlot">EL SLOT AL QUE SE MOVERAN LOS ITEMS</param>
    private void MoveItem(Slot originSlot, Slot destinationSlot)
    {
        if (originSlot.Item!=null)
        {
            destinationSlot.AddItem(originSlot.Item, originSlot.Amount);
            destinationSlot.Item.SetSlot(destinationSlot);
            EmptySlot(originSlot);
        }
        else
        {
            Debug.LogWarning("origin slot was null");
        }
    }
   
    /// <summary>
    /// Llena un slot dado por un item dado
    /// </summary>
    /// <param name="slot">Slot a llenar</param>
    /// <param name="itemToAdd">Tipo de item que se colocar� en el slot</param>
    public void FillSlot(Slot slot)
    {
        slot.Fill();
    }
  
     /// <summary>
     /// Se consumen items del slot y se actualiza su informacion
     /// </summary>
     /// <param name="slot">Slot del que se consumen objetos, cantidad de consumo es dada por el item</param>
    public void ConsumeItem(Slot slot)
    {
        slot.RemoveItems(slot.Item.UnitsPerUse);
        if (slot.Amount <= 0)
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
        Slot lessAmmoSlot = null;// = slots[0,0];
        
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (!slots[i, j].IsEmpty()) //Si tiene algun item:
                {
                    if (slots[i, j].ItemType == ItemTypes.AMMO)
                    {
                        if (lessAmmoSlot == null) // Si no se habia encontrado un slot que sea el de menor cantidad de balas, entonces es el actual
                        {
                            lessAmmoSlot = slots[i, j];
                        }
                        if (lessAmmoSlot.Amount >= slots[i, j].Amount) // Si el slot actual tiene menos balas que el que antes era el que menos tenia, ahora este es el que menos tiene
                            //           mayor o igual para que sea el ultimo encontrado si es que tienen varios la misma cantidad
                        {
                            lessAmmoSlot = slots[i, j];
                        }
                    }
                }
            }
        }
        return lessAmmoSlot;
    }
    /// <summary>
    /// Agrega municion al cargador del Player, buscar� hasta encontrar mas municion si se vacia el slot
    /// </summary>
    public int GetAmmoFromInventory(int ammoRequested)
    {
        Slot newSlot=SearchAmmo();

        if (newSlot==null)
        {
            return 0;
        }
        else
        {
            int ammoFound = newSlot.Amount;
            
            if (ammoFound==ammoRequested) // si se encuentra un slot con las mismas balas que las pedidas
            {
                EmptySlot(newSlot);
                return ammoFound;
            }
            
            if (ammoFound>ammoRequested) //si el slot tiene mas balas de las requeridas:
            {
                newSlot.Amount -= ammoRequested;
                return ammoRequested;
            }
            else // si el slot tiene menos balas
            {
                int ammoRequestedLeft = ammoRequested - ammoFound;
                EmptySlot(newSlot);
            
                return ammoFound + GetAmmoFromInventory(ammoRequestedLeft);
            }
        }

        return 0;
    }
}//class