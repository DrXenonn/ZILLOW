using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    [SerializeField] private GameObject Object;

    private void Start()
    {
        Instantiate(Object);
    }
}
