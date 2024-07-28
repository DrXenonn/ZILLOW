using System.Collections;
using TMPro;
using UnityEngine;


public class PotInteraction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;

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
        
    }
}