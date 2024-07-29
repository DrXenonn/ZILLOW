using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryData PlayerInventory;
    [SerializeField] private GameObject UIObject;
    [SerializeField] private Image[] Items;
    [SerializeField] private TextMeshProUGUI[] Texts;
    [SerializeField] private Image[] Arrows;
    [SerializeField] private TextMeshProUGUI[] ArrowTexts;
    [SerializeField] private Image[] Potions;
    [SerializeField] private TextMeshProUGUI[] PotionTexts;


    private void RefreshInventory()
    {
        var index = 0;
        var arrowIndex = 0;
        var potionIndex = 0;
        foreach (var item in PlayerInventory.Inventory.Keys)
        {
            if (item.ItemType is ItemTypes.Ingredient or ItemTypes.Seed)
            {
                Items[index].sprite = item.Icon;
                Texts[index].text = PlayerInventory.Inventory[item].ToString();
                index++;
            }
            else if (item.ItemType == ItemTypes.Arrow)
            {
                Arrows[arrowIndex].sprite = item.Icon;
                ArrowTexts[arrowIndex].text = PlayerInventory.Inventory[item].ToString();
                arrowIndex++;
            }
            else
            {
                Potions[potionIndex].sprite = item.Icon;
                PotionTexts[potionIndex].text = PlayerInventory.Inventory[item].ToString();
                potionIndex++;
            }
        }
    }

    public void OnItemPickUp(Component sender, object data)
    {
        Invoke(nameof(RefreshInventory), 0.1f);
    }

    public void OnShoot(Component sender, object data)
    {
        Invoke(nameof(RefreshInventory), 0.1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            RefreshInventory();
            UIObject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            UIObject.SetActive(false);
        }
    }
}