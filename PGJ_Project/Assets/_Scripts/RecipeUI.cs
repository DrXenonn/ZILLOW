using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] private Transform Parent;
    [SerializeField] private Recipes RecipeManager;
    [SerializeField] private Sprite PlusSign, ArrowSign, QuestionMark;
    [SerializeField] private GameObject RecipeUIPrefab, Icon;
    [SerializeField] private InventoryData PlayerInventory;
    [SerializeField] private GameObject CraftButton;
    [SerializeField] private Image ProgressBar;
    [SerializeField] private TextMeshProUGUI ProgressText, ActionText;
    [SerializeField] private Transform ItemSpawnPoint;
    private GameObject _currentHighlight;
    private Recipe _selectedRecipe;
    private bool _isCrafting;
    
    [Header("Sound")]
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private float MinPitch, MaxPitch;

    private void Start()
    {
        UpdateRecipes();
        SetCraftButtonState(false);
    }

    public void OnOpenMenu(Component sender, object data)
    {
        if (sender != this) return;
        UpdateRecipes();
        SetCraftButtonState(false);
    }

    private void UpdateRecipes()
    {
        for (var i = 0; i < Parent.childCount; i++)
        {
            Destroy(Parent.GetChild(i).gameObject);
        }
        _currentHighlight = null;

        foreach (var recipe in RecipeManager.RecipesArray)
        {
            var r = Instantiate(RecipeUIPrefab, Parent);
            var evenIndex = 0;
            var foundAllItems = true;
            var availableItems = 0;

            for (var i = 0; i < (recipe.Ingredients.Length * 2) - 1; i++)
            {
                var icon = Instantiate(Icon, r.transform);
                var border = icon.GetComponent<Image>();
                var desc = icon.GetComponent<ShowDescription>();
                if (i % 2 == 0)
                {
                    var item = recipe.Ingredients[evenIndex].Item;
                    var quantity = recipe.Ingredients[evenIndex].Quantity;
                    var iconComponent = icon.transform.GetChild(0).GetComponent<Image>();
                    if (PlayerInventory.Inventory.ContainsKey(item))
                    {
                        iconComponent.sprite = item.Icon;
                        desc.Description.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
                        PrintQuantity(icon, recipe.Ingredients[evenIndex].Quantity);

                        var hasEnough = PlayerInventory.Inventory[item] >= quantity;
                        availableItems += hasEnough ? 1 : 0;
                        var color = iconComponent.color;
                        color.a = hasEnough ? 1 : 0.5f;
                        iconComponent.color = color;
                    }
                    else
                    {
                        iconComponent.sprite = QuestionMark;
                        PrintQuantity(icon, recipe.Quantity);
                        desc.ShowDesc = false;
                        foundAllItems = false;
                    }

                    evenIndex++;
                }
                else
                {
                    icon.transform.GetChild(0).GetComponent<Image>().sprite = PlusSign;
                    desc.ShowDesc = false;
                    border.enabled = false;
                }
            }

            var arrowIcon = Instantiate(Icon, r.transform);
            arrowIcon.transform.GetChild(0).GetComponent<Image>().enabled = false;
            arrowIcon.GetComponent<Image>().sprite = ArrowSign;
            arrowIcon.GetComponent<ShowDescription>().ShowDesc = false;


            var resultIcon = Instantiate(Icon, r.transform);
            var resultIconComponent = resultIcon.transform.GetChild(0).GetComponent<Image>();
            var description = resultIcon.GetComponent<ShowDescription>();
            if (foundAllItems)
            {
                resultIconComponent.sprite = recipe.Result.Icon;
                PrintQuantity(resultIcon, recipe.Quantity);
                var color = resultIconComponent.color;
                var button = resultIcon.GetComponent<Button>();
                description.Description.GetComponentInChildren<TextMeshProUGUI>().text = recipe.Result.name;
                if (availableItems >= recipe.Ingredients.Length)
                {
                    color.a = 1;
                    button.enabled = true;
                    button.onClick.AddListener(delegate { OnClick(recipe, button.transform.GetChild(1).gameObject); });
                }
                else
                {
                    color.a = 0.5f;
                    button.enabled = false;
                }

                resultIconComponent.color = color;
            }
            else
            {
                resultIconComponent.sprite = QuestionMark;
                resultIcon.GetComponent<ShowDescription>().ShowDesc = false;
            }
        }
    }

    private void OnClick(Recipe selectedRecipe, GameObject highlight)
    {
        if (_isCrafting) return;

        _currentHighlight?.SetActive(false);
        highlight.SetActive(true);
        _currentHighlight = highlight;
        _selectedRecipe = selectedRecipe;

        SetCraftButtonState(true);
    }

    public void OnCraft()
    {
        if (_currentHighlight == null) return;
        
        _isCrafting = true;
        CraftButton.SetActive(false);
        ProgressBar.transform.parent.gameObject.SetActive(true);
        SetCraftButtonState(false);
        _currentHighlight.SetActive(false);

        foreach (var ingredient in _selectedRecipe.Ingredients)
        {
            PlayerInventory.Inventory[ingredient.Item] -= ingredient.Quantity;
        }
        
        UpdateRecipes();
        StartCoroutine(nameof(FillProgressBar));

        AudioSource.pitch = Random.Range(MinPitch, MaxPitch);
        AudioSource.Play();
    }

    private void OnFinishProgressBar()
    {
        _isCrafting = false;
        ProgressBar.transform.parent.gameObject.SetActive(false);
        CraftButton.SetActive(true);
        SetCraftButtonState(false);
        StartCoroutine(nameof(SpawnItems));
        AudioSource.Stop();
    }

    private IEnumerator SpawnItems()
    {
        var itemPrefab = _selectedRecipe.Result.ModelPrefab;
        for (var i = 0; i < _selectedRecipe.Quantity; i++)
        {
            yield return new WaitForSeconds(0.07f);
            Instantiate(itemPrefab, ItemSpawnPoint.position, quaternion.identity);
        }
    }

    private IEnumerator FillProgressBar()
    {
        var time = 0.0f;
        var duration = _selectedRecipe.Time;
        while (time < duration)
        {
            ProgressBar.fillAmount = time / Mathf.Max(duration, Mathf.Epsilon);
            time += Time.deltaTime;
            ProgressText.text = Mathf.RoundToInt(ProgressBar.fillAmount / 1 * 100) + "%";
            yield return new WaitForEndOfFrame();
        }

        OnFinishProgressBar();
    }

    private void SetCraftButtonState(bool state)
    {
        CraftButton.GetComponent<Button>().enabled = state;
        ActionText.text = state ? "Craft" : "Select an item first...";
    }

    public void OnCollectItem(Component sender, object data)
    {
        UpdateRecipes();
    }

    private void PrintQuantity(GameObject icon, int quantity)
    {
        var tmp = icon.GetComponentInChildren<TextMeshProUGUI>();
        tmp.enabled = true;
        tmp.text = "x" + quantity;
    }
}