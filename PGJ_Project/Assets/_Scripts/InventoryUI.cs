using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryData PlayerInventory;
    [SerializeField] private GameObject UIObject;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UIObject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            UIObject.SetActive(false);
        }
    }
}