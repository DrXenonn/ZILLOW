using UnityEngine;

public class PoisonPotion : MonoBehaviour
{
    [SerializeField] private float Time;
    [SerializeField] private ParticleSystem ParticleSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Enemy"))
        {
            ParticleSystem.gameObject.transform.parent = null;
            ParticleSystem.Play();
            Destroy(ParticleSystem.gameObject, Time);
            Destroy(gameObject, 1.5f);
        }
    }
}
