using UnityEngine;

public class DarkArea : MonoBehaviour
{
    [SerializeField] private BoolVariable IsInShadow, IsNight;
    private void OnTriggerStay(Collider other)
    {
        if (!IsInShadow.Value && !other.CompareTag("Player")) return;
        IsInShadow.Value = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && IsNight.Value) return;
        IsInShadow.Value = false;
    }
}
