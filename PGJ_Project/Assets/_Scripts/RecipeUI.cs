using UnityEngine;
using Image = UnityEngine.UI.Image;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] private Transform Parent;
    [SerializeField] private Recipes RecipeManager;
    [SerializeField] private Sprite PlusSign, ArrowSign, QuestionMark;
    [SerializeField] private GameObject RecipeUIPrefab, Icon;
    private void Start()
    {
        foreach (var recipe in RecipeManager.RecipesArray)
        {
            var r = Instantiate(RecipeUIPrefab, Parent);
            var evenIndex = 0;
            for (var i = 0; i < (recipe.Ingredients.Length * 2) - 1; i++)
            {
                var icon = Instantiate(Icon, r.transform);
                if (i % 2 == 0)
                {
                    icon.GetComponent<Image>().sprite = recipe.Ingredients[evenIndex].Icon;
                    evenIndex++;
                }
                else
                {
                    icon.GetComponent<Image>().sprite = PlusSign;
                }
            }
            var arrow = Instantiate(Icon, r.transform);
            arrow.GetComponent<Image>().sprite = ArrowSign;
            var result = Instantiate(Icon, r.transform);
            result.GetComponent<Image>().sprite = recipe.Result.Icon;
        }
    }
}