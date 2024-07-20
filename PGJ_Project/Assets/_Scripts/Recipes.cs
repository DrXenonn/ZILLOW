using UnityEngine;

[System.Serializable]
public struct Recipe
{
    public Item[] Ingredients;
    public Item Result;
}

public class Recipes : MonoBehaviour
{
    public Recipe[] RecipesArray;
}