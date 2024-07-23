using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private LayerMask InteractableLayer;
    [SerializeField] private Transform RayOrigin;
    [SerializeField] private float InteractionRange;
    private Interactable _currentTarget;
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Physics.Raycast(RayOrigin.transform.position, _cam.transform.forward, out var hit, 6, InteractableLayer)) //TODO it goes through stuff
        {
            if (!_currentTarget)
            {
                _currentTarget = hit.collider.GetComponent<Interactable>();
                _currentTarget.SetTextState(true);
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                _currentTarget.OnInteract();
            }
        }
        else
        {
            if (_currentTarget)
            {
                _currentTarget.SetTextState(false);
                _currentTarget = null;
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(RayOrigin.transform.position, _cam!.transform.forward);
    }
}

