using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject HitEffect;
    [SerializeField] private int Damage;
    [SerializeField] private float TimeToLive;
    [SerializeField] private bool IsPoison;
    private bool _hit;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_hit) return;

        if (other.CompareTag("Enemy"))
        {
            _hit = true;
            var enemyHealth = other.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(Damage);
            if (!IsPoison) return;

            enemyHealth.SetPoison(true);
            Destroy(gameObject, 0.05f);
        }
        else if (other.CompareTag("Environment") || other.CompareTag("Ground"))
        {
            _hit = true;
            Destroy(gameObject, 0.05f);
        }
    }

    private void Start()
    {
        Destroy(gameObject, TimeToLive);
    }

    private void OnDestroy()
    {
        HitEffect.SetActive(true);
        HitEffect.transform.parent = null;
    }
}