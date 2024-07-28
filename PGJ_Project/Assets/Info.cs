using System.Collections;
using TMPro;
using UnityEngine;

public class Info : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private float InfoTime;
    private string _receivedText;
    
    public void OnInfo(Component sender, object data)
    {
        var text = (string)data;
        _receivedText = text;
        StopAllCoroutines();
        StartCoroutine(nameof(ShowInfo));
    }

    private IEnumerator ShowInfo()
    {
        Text.text = _receivedText;
        Text.enabled = true;
        yield return new WaitForSeconds(InfoTime);
        Text.enabled = false;
    }
}
