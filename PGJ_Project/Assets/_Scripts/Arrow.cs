using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private int Damage;
    [SerializeField] private float TimeToLive;
    [SerializeField] private bool IsPoison;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemyHealth = other.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(Damage);
            if (!IsPoison) return;

            enemyHealth.SetPoison(true);
            Destroy(gameObject, 0.05f);
        }
        else if (other.CompareTag("Environment") || other.CompareTag("Ground"))
        {
            Destroy(gameObject, 0.05f);
        }
    }

    private void Start()
    {
        Destroy(gameObject, TimeToLive);
    }
}