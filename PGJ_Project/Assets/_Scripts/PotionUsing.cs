using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PotionUIs
{
    public Item PotionItem;
    public Image Icon;
    public TextMeshProUGUI Text;
    public GameObject Highlight;
    public GameObject PotionPrefab;
}

public class PotionUsing : MonoBehaviour
{
    [SerializeField] private GameObjectVariable SelectedPotion;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private BoolVariable CanShoot;
    [SerializeField] private GameEvent OnShoot;
    [SerializeField] private InventoryData PlayerInventory;
    [SerializeField] private PotionUIs[] PotionUIs;
    [SerializeField] private AudioSource SwitchAudio;
    [SerializeField] private GameEvent OnInfo;
    [SerializeField] private float FireForce;
    private Item _selectedPotionItem;
    private Camera _cam;
    
    private void Awake()
    {
        _cam = Camera.main;
        CanShoot.Value = true;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanShoot.Value && _selectedPotionItem is not null && Cursor.lockState == CursorLockMode.Locked)
        {
            if (PlayerInventory.Inventory[_selectedPotionItem] > 0)
            {
                var arrow = Instantiate(SelectedPotion.Value, FirePoint.transform.position, quaternion.identity);
                arrow.GetComponent<Rigidbody>().AddForce((_cam.transform.forward) * FireForce);
                OnShoot.Raise();
                PlayerInventory.Inventory[_selectedPotionItem] -= 1;
                UpdateUI();
            }
            else
            {
                OnInfo.Raise("Out of " + _selectedPotionItem.name + "s!");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!PlayerInventory.Inventory.ContainsKey(PotionUIs[0].PotionItem)) return;
            EnableHighlight(0);
            SelectedPotion.Value = PotionUIs[0].PotionPrefab;
            _selectedPotionItem = PotionUIs[0].PotionItem;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!PlayerInventory.Inventory.ContainsKey(PotionUIs[1].PotionItem)) return;
            EnableHighlight(1);
            SelectedPotion.Value = PotionUIs[1].PotionPrefab;
            _selectedPotionItem = PotionUIs[1].PotionItem;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!PlayerInventory.Inventory.ContainsKey(PotionUIs[2].PotionItem)) return;
            EnableHighlight(2);
            SelectedPotion.Value = PotionUIs[2].PotionPrefab;
            _selectedPotionItem = PotionUIs[2].PotionItem;
        }
    }

    private void UpdateUI()
    {
        foreach (var potionUI in PotionUIs)
        {
            if (!PlayerInventory.Inventory.ContainsKey(potionUI.PotionItem)) continue;
            
            potionUI.Icon.sprite = potionUI.PotionItem.UIIcon;
            potionUI.Text.text = PlayerInventory.Inventory[potionUI.PotionItem].ToString();
        }
    }

    private void EnableHighlight(int index)
    {
        if (PotionUIs[index].Highlight.activeSelf) return;
        
        foreach (var arrowUI in PotionUIs)
        {
            arrowUI.Highlight.SetActive(false);
        }
        PotionUIs[index].Highlight.SetActive(true);
        SwitchAudio.PlayOneShot(SwitchAudio.clip);
    }

    public void OnItemPickup(Component sender, object data)
    {
        Invoke(nameof(UpdateUI), 0.1f);
    }
}