using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Image HealthBar, PoisonImage;
    [SerializeField] private FloatVariable PoisonDamage, PoisonDuration;
    [SerializeField] private EnemyMovement Movement;
    [SerializeField] private Item ItemDrop;
    [SerializeField] private Animator Animator;
    [SerializeField] private MeleeAttack MeleeAttack;
    [SerializeField] private GameObject HpBarObject;
    [SerializeField] private BoxCollider Collider;
    [SerializeField] private GameEvent OnWin;
    [SerializeField] private bool IsBoss;
    [SerializeField] private int Health;
    private bool _poison;
    private bool _dead;
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
        HpBarObject.SetActive(true);
        Health -= value;
        HealthBar.fillAmount = (float)Health / _maxHealth;
        Movement.FollowPlayer();
        HandleDeath();
    }

    private void HandleDeath()
    {
        if (Health <= 0 && !_dead)
        {
            _dead = true;
            Instantiate(ItemDrop.ModelPrefab, transform.position + (Vector3.up * 2), Quaternion.Euler(-90, 0 ,0));
            MeleeAttack.enabled = false;
            Movement.enabled = false;
            Animator.CrossFade("Death", 0);
            Collider.enabled = false;
            Destroy(gameObject, 1f);
            if (IsBoss)
            {
                OnWin.Raise();
            }
        }
    }

    public void OnPlayerDeath(Component sender, object data)
    {
        if (IsBoss)
        {
            Health = _maxHealth;
            HealthBar.fillAmount = (float)Health / _maxHealth;
        }
    }
}