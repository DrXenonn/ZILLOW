using System.Collections;
using TMPro;
using UnityEngine;

public enum InteractionTypes
{
    UseTool,
    Plant,
    Quest
}

public class Interactable : MonoBehaviour
{
    [SerializeField] private InteractionTypes InteractionType;
    [SerializeField] private GameObject ToolUI;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private PlayerLook PlayerLook;
    [SerializeField] private BoolVariable CanMove;
    [SerializeField] private GameEvent OnOpenMenu;
    
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
        if (InteractionType == InteractionTypes.UseTool)
        {
            PlayerLook.LockAndHideMouse(false);
            CanMove.Value = false;
            ToolUI.SetActive(true);
            PlayerLook.LockCamera(true);
            OnOpenMenu.Raise(ToolUI.transform.parent.GetChild(0).GetComponent<RecipeUI>());
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
