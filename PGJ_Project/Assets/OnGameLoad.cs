using UnityEngine;

public class OnGameLoad : MonoBehaviour
{

    [SerializeField] private Light[] Lights;

    private void Start()
    {
        Invoke(nameof(LateStart),0.5f);
    }

    private void LateStart()
    {
        foreach (var l in Lights)
        {
            l.enabled = true;
        }
    }
}
