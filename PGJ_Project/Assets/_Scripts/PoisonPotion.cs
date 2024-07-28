using Unity.Mathematics;
using UnityEngine;

public class PoisonPotion : MonoBehaviour
{
    [SerializeField] private float Time;
    [SerializeField] private GameObject PoisonArea;

    private void Start()
    {
        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Enemy") || other.CompareTag("Environment"))
        {
            Instantiate(PoisonArea, transform.position, Quaternion.Euler(270, 0, 0));
            Destroy(gameObject);
        }
    }
}