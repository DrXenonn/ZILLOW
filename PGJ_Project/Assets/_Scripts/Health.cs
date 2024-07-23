using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Image BlackImage;
    [SerializeField] private GameObject LowHpWarning;
    [SerializeField] private FloatVariable PlayerHealth;
    [SerializeField] private Volume Volume;
    [SerializeField] private GameObject DarknessBar;
    [SerializeField] private BoolVariable IsInShadow;
    [SerializeField] private Image Bar1, Bar2;
    [SerializeField] private float LowHp;
    [SerializeField] private float MaxHealth, MaxVignette, MaxVignetteSmoothness, MaxImageDarkness;
    private float _originalVignette, _originalVignetteSmoothness;
    private Color _color;
    private Vignette _vignette;
    
    private void Start()
    {
        PlayerHealth.Value = MaxHealth;
        _color = BlackImage.color;
        
        if (Volume.profile.TryGet(out _vignette))
        {
            _originalVignette = _vignette.intensity.value;
            _originalVignetteSmoothness = _vignette.smoothness.value;
        }
        InvokeRepeating(nameof(HandleShadow), 0.1f, 0.1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10);
        }
    }

    private void TakeDamage(int damage)
    {
        PlayerHealth.Value = Mathf.Max(PlayerHealth.Value - damage, 0);
        UpdateEffects();
    }

    private void UpdateEffects()
    {
        var healthRatio = PlayerHealth.Value / MaxHealth;

        _vignette.intensity.value = Mathf.Lerp(MaxVignette, _originalVignette, healthRatio);
        _vignette.smoothness.value = Mathf.Lerp(MaxVignetteSmoothness, _originalVignetteSmoothness, healthRatio);

        var newAlphaValue = Mathf.Lerp(MaxImageDarkness, 0, healthRatio);
        _color.a = newAlphaValue;
        BlackImage.color = _color;
        HandleDarknessBar();
        HandleLowHpWarning();
    }

    private void HandleDarknessBar()
    {
        DarknessBar.SetActive(PlayerHealth.Value < MaxHealth);
        Bar1.fillAmount = 1 - PlayerHealth.Value / MaxHealth;
        Bar2.fillAmount = 1 - PlayerHealth.Value / MaxHealth;
    }

    private void HandleLowHpWarning()
    {
        LowHpWarning.SetActive(PlayerHealth.Value < LowHp);
    }

    private void Heal(int value)
    {
        if (PlayerHealth.Value >= MaxHealth) return;
        
        PlayerHealth.Value = Mathf.Min(PlayerHealth.Value + value, MaxHealth);
        UpdateEffects();
    }

    private void HandleShadow()
    {
        if (IsInShadow.Value)
        {
            TakeDamage(2);
        }
        else
        {
            Heal(1);
        }
    }
}