using System.Collections;
using TMPro;
using UnityEngine;

public enum InteractionType
{
    Tool,
    Pot,
    Loot
}

public class Interactable : MonoBehaviour
{
    [SerializeField] private InteractionType Type;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private InventoryData PlayerInventory;
    [Header("Tools")]
    [SerializeField] private GameObject ToolUI;
    [SerializeField] private PlayerLook PlayerLook;
    [SerializeField] private BoolVariable CanMove;
    [SerializeField] private GameEvent OnOpenMenu;
    [Header("Pot")] 
    [SerializeField] private Planting PlantingComponent;
    private IEnumerator Start()
    {
        Text.enabled = true;
        yield return new WaitForSeconds(0.1f);
        Text.enabled = false;
    }

    public void SetTextState(bool state)
    {
        Text.enabled = state;
    }

    public void OnInteract()
    {
        if (Type == InteractionType.Tool)
        {
            PlayerLook.LockAndHideMouse(false);
            CanMove.Value = false;
            ToolUI.SetActive(true);
            PlayerLook.LockCamera(true);
            OnOpenMenu.Raise(ToolUI.transform.parent.GetChild(0).GetComponent<RecipeUI>());
        }
        else if (Type == InteractionType.Pot)
        {
            PlantingComponent.OnInteract();
        }
    }

    public void OnCloseToolUI()
    {
        ToolUI.SetActive(false);
        PlayerLook.LockCamera(false);
        CanMove.Value = true;
        PlayerLook.LockAndHideMouse(true);
    }
}