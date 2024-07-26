using Unity.Mathematics;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    [SerializeField] private GameObjectVariable SelectedArrow;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private BoolVariable CanShoot;
    [SerializeField] private GameEvent OnShoot;
    [SerializeField] private float FireForce;
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanShoot.Value)
        {
            var arrow = Instantiate(SelectedArrow.Value, FirePoint.transform.position, quaternion.identity);
            arrow.GetComponent<Rigidbody>().AddForce((_cam.transform.forward) * FireForce, ForceMode.Impulse);
            OnShoot.Raise();
        }
    }
}