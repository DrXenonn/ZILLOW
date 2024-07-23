using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryData PlayerInventory;
    [SerializeField] private GameEvent Notification;

    public void OnCollectItem(Component sender, object data)
    {
        var item = (Item)data;
        
        if (!PlayerInventory.Inventory.ContainsKey(item))
        {
            PlayerInventory.Inventory.Add(item, 1);
            var itemData = new Tuple<Item, bool>(item, true);
            Notification.Raise(itemData);
        }
        else
        {
            PlayerInventory.Inventory[item] += 1;
            var itemData = new Tuple<Item, bool>(item, false);
            Notification.Raise(itemData);
        }
    }
}
