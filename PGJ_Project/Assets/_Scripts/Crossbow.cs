using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ArrowUI
{
    public Item ArrowItem;
    public Image Icon;
    public TextMeshProUGUI Text;
    public TextMeshProUGUI HighlightText;
    public GameObject Highlight;
    public GameObject ArrowPrefab;
}

public class Crossbow : MonoBehaviour
{
    [SerializeField] private GameObjectVariable SelectedArrow;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private BoolVariable CanShoot;
    [SerializeField] private GameEvent OnShoot;
    [SerializeField] private InventoryData PlayerInventory;
    [SerializeField] private ArrowUI[] ArrowUIs;
    [SerializeField] private AudioSource SwitchAudio;
    [SerializeField] private GameEvent OnInfo;
    [SerializeField] private float FireForce;
    private Item _selectedArrowItem;
    private Camera _cam;
    
    private void Awake()
    {
        _cam = Camera.main;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanShoot.Value && _selectedArrowItem is not null && Cursor.lockState == CursorLockMode.Locked)
        {
            if (PlayerInventory.Inventory[_selectedArrowItem] > 0)
            {
                var arrow = Instantiate(SelectedArrow.Value, FirePoint.transform.position, quaternion.identity);
                arrow.GetComponent<Rigidbody>().AddForce((_cam.transform.forward) * FireForce, ForceMode.Impulse);
                OnShoot.Raise();
                PlayerInventory.Inventory[_selectedArrowItem] -= 1;
                UpdateUI();
            }
            else
            {
                OnInfo.Raise("Out of " + _selectedArrowItem.name + "!");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!PlayerInventory.Inventory.ContainsKey(ArrowUIs[0].ArrowItem)) return;
            EnableHighlight(0);
            SelectedArrow.Value = ArrowUIs[0].ArrowPrefab;
            _selectedArrowItem = ArrowUIs[0].ArrowItem;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!PlayerInventory.Inventory.ContainsKey(ArrowUIs[1].ArrowItem)) return;
            EnableHighlight(1);
            SelectedArrow.Value = ArrowUIs[1].ArrowPrefab;
            _selectedArrowItem = ArrowUIs[1].ArrowItem;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!PlayerInventory.Inventory.ContainsKey(ArrowUIs[2].ArrowItem)) return;
            EnableHighlight(2);
            SelectedArrow.Value = ArrowUIs[2].ArrowPrefab;
            _selectedArrowItem = ArrowUIs[2].ArrowItem;
        }
    }

    private void UpdateUI()
    {
        foreach (var arrowUI in ArrowUIs)
        {
            if (!PlayerInventory.Inventory.ContainsKey(arrowUI.ArrowItem)) continue;
            
            arrowUI.Icon.sprite = arrowUI.ArrowItem.UIIcon;
            arrowUI.Text.text = PlayerInventory.Inventory[arrowUI.ArrowItem].ToString();
            arrowUI.HighlightText.text = PlayerInventory.Inventory[arrowUI.ArrowItem].ToString();
        }
    }

    private void EnableHighlight(int index)
    {
        if (ArrowUIs[index].Highlight.activeSelf) return;

        foreach (var arrowUI in ArrowUIs)
        {
            arrowUI.Highlight.SetActive(false);
        }
        ArrowUIs[index].Highlight.SetActive(true);
        SwitchAudio.PlayOneShot(SwitchAudio.clip);
    }

    public void OnItemPickup(Component sender, object data)
    {
        Invoke(nameof(UpdateUI), 0.1f);
    }
}