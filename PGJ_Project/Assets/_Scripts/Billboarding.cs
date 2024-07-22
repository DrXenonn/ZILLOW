using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private Transform _cam;

    private void Awake()
    {
        _cam = Camera.main?.transform;
    }

    private void Update()
    {
        transform.forward = _cam.forward;
    }
}
