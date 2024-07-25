using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [SerializeField] private BoolVariable IsInSafeZone;
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        IsInSafeZone.Value = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        IsInSafeZone.Value = false;
    }
}