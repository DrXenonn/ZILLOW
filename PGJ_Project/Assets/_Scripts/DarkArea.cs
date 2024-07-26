using UnityEngine;

public class DarkArea : MonoBehaviour
{
    [SerializeField] private BoolVariable IsInDarkArea;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        IsInDarkArea.Value = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        IsInDarkArea.Value = false;
    }
}