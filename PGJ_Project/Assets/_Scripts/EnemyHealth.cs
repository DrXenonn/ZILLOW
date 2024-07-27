using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Image HealthBar, PoisonImage;
    [SerializeField] private FloatVariable PoisonDamage, PoisonDuration;
    [SerializeField] private EnemyMovement Movement;
    [SerializeField] private int Health;
    private bool _poison;
    private int _maxHealth;

    private void Start()
    {
        _maxHealth = Health;
    }

    private IEnumerator HandlePoison()
    {
        var elapsed = 0f;

        while (elapsed < PoisonDuration.Value)
        {
            TakeDamage((int)PoisonDamage.Value);
            elapsed += 0.25f;
            yield return new WaitForSeconds(0.25f);
        }

        SetPoison(false);
    }

    public void SetPoison(bool state)
    {
        _poison = state;

        if (_poison)
        {
            PoisonImage.enabled = true;
            StopAllCoroutines();
            StartCoroutine(HandlePoison());
        }
        else
        {
            PoisonImage.enabled = false;
            StopAllCoroutines();
        }
    }

    public void TakeDamage(int value)
    {
        Health -= value;
        HealthBar.fillAmount = (float)Health / _maxHealth;
        Movement.FollowPlayer();
        HandleDeath();
    }

    private void HandleDeath()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}