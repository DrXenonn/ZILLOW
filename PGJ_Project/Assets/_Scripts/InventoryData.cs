using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory", fileName = "PlayerInventory")]
public class InventoryData : ScriptableObject
{
    public List<Item> Items = new List<Item>();
}