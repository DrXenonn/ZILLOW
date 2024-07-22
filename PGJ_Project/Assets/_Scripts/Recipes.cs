using UnityEngine;

[System.Serializable]
public struct Ingredient
{
    public Item Item;
    public int Quantity;
}

[System.Serializable]
public struct Recipe
{
    public Ingredient[] Ingredients;
    public Item Result;
    public int Quantity;
    public float Time;
}

[CreateAssetMenu(menuName = "RecipeManager", fileName = "RecipeManager")]
public class Recipes : ScriptableObject
{
    public Recipe[] RecipesArray;
}