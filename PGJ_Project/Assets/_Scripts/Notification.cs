using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI TMP;
    [SerializeField] private float Duration;

    public void OnNewItemCollect(Component sender, object data)
    {
        var itemData = (Tuple<Item, bool>)data;
        CancelInvoke(nameof(DisableNotification));
        EnableNotification();
        
        if (itemData.Item2)
        {
            Icon.sprite = itemData.Item1.Icon;
            TMP.text = $"New {itemData.Item1.ItemType}:<b> {itemData.Item1.name}";
        }
        else
        {
            Icon.sprite = itemData.Item1.Icon;
            TMP.text = $"Added to inventory";
        }
        
        Invoke(nameof(DisableNotification), Duration);
    }

    private void EnableNotification()
    {
        Icon.enabled = true;
        TMP.enabled = true;
    }

    private void DisableNotification()
    {
        Icon.enabled = false;
        TMP.enabled = false;
    }
}