using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory", fileName = "PlayerInventory")]
public class InventoryData : ScriptableObject
{
    public SerializedDictionary<Item, int> Inventory = new SerializedDictionary<Item, int>();
}