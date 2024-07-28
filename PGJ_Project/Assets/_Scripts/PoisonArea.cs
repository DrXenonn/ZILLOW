using UnityEngine;

public class PoisonArea : MonoBehaviour
{
    [SerializeField] private float Time;
    [SerializeField] private SphereCollider Collider;

    private void Start()
    {
        InvokeRepeating(nameof(ResetCollider),0.25f, 0.25f);
        Destroy(gameObject, Time);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().SetPoison(true);
        }
    }

    private void ResetCollider()
    {
        Collider.enabled = false;
        Collider.enabled = true;
    }
}
