using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
   AMMO, HEAL, KEY, NONE
}
public interface IItem
{
   ItemTypes ItemType { get; }
   int Amount { get; }
   int MaxStackeable { get; }
   int UnitsPerUse { get; }
   Sprite ItemSprite { get; }
   void Use();
   void AddToInventory(Collider2D player);
}

public static class ItemConstants
{
   public static string USE_AMMO = "UseAmmo";
   public static string USE_HEAL = "UseHeal";
   public static string ITEM_TAG = "Item";
}