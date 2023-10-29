using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
   AMMO, HEAL, KEY, NONE
}
public interface IItem
{
   Slot ItemSlot { get; }
   ItemTypes ItemType { get; }
   int Amount { get; }
   int MaxStackeable { get; }
   int UnitsPerUse { get; }
   Sprite ItemSprite { get; }
   void UseItem();
   void AddToInventory(Collider2D player);
   void SetSlot(Slot newSlot);
}

public static class ItemConstants
{
   public static string USE_AMMO = "UseAmmo";
   public static string USE_HEAL = "UseHeal";
   public static string ITEM_TAG = "Item";
   public static string DISCARD = "Discard";
}