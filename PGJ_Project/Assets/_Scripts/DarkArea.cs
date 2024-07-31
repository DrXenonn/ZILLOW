using UnityEngine;

public class DarkArea : MonoBehaviour
{
    [SerializeField] private BoolVariable IsInDarkArea;
    [SerializeField] private GameEvent OnEnterDarkArea, OnExitDarkArea;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnEnterDarkArea.Raise();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        IsInDarkArea.Value = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        IsInDarkArea.Value = false;
        OnExitDarkArea.Raise();
    }
}