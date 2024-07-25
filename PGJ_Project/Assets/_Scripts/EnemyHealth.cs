using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int Health;

    public void TakeDamage(int value)
    {
        Health -= value;
    }

    private void HandleDeath()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
