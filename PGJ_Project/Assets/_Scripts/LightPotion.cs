using UnityEngine;

public class LightPotion : MonoBehaviour
{
    [SerializeField] private int HealAmount;
    [SerializeField] private int Damage;
    [SerializeField] private GameEvent HealPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealPlayer.Raise(HealAmount);
        }

        if (other.CompareTag("Enemy"))
        {
            var enemyHealth = other.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(Damage);
        }
    }

    private void Start()
    {
        Destroy(gameObject, 1);
    }
}