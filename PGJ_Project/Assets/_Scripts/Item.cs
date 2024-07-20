using UnityEngine;

public enum ItemTypes
{
    Ingredient,
    Consumable,
    Throwable,
    Seed
}

[CreateAssetMenu(menuName = "Item", fileName = "NewItem")]
public class Item : ScriptableObject
{
    public int ID;
    public ItemTypes ItemType;
    public string Name;
    public string Description;
    public bool IsDiscovered;
    public Sprite Icon;
    public GameObject ModelPrefab;
    public GameObject LogicPrefab;
}