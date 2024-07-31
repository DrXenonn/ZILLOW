using System.Collections;
using TMPro;
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
    [SerializeField] private BoolVariable IsNight, IsInSafeZone, IsInDarkArea;
    [SerializeField] private Image Bar1, Bar2;
    [SerializeField] private Image ADImage, ADBack;
    [SerializeField] private TextMeshProUGUI ADText;
    [SerializeField] private GameEvent OnDeath;
    [SerializeField] private BoolVariable IsDead;
    [SerializeField] private float LowHp;
    [SerializeField] private float MaxHealth, MaxVignette, MaxVignetteSmoothness, MaxImageDarkness;
    [SerializeField] private int DarknessDamageTick;
    [SerializeField] private int HealTick;
    private float _originalVignette, _originalVignetteSmoothness;
    private float _antiDarknessTime;
    private Color _color;
    private Vignette _vignette;

    private void Start()
    {
        IsDead.Value = false;
        PlayerHealth.Value = MaxHealth;
        _color = BlackImage.color;
        IsNight.Value = false;
        IsInSafeZone.Value = false;
        IsInDarkArea.Value = false;

        if (Volume.profile.TryGet(out _vignette))
        {
            _originalVignette = _vignette.intensity.value;
            _originalVignetteSmoothness = _vignette.smoothness.value;
        }

        InvokeRepeating(nameof(HandleShadow), 0.05f, 0.05f);
        InvokeRepeating(nameof(HandleAntiDarkness),1, 1);
    }

    private void TakeDamage(int damage)
    {
        PlayerHealth.Value = Mathf.Max(PlayerHealth.Value - damage, 0);
        CheckDeath();
        UpdateEffects();
    }

    private void CheckDeath()
    {
        if (PlayerHealth.Value <= 0 && !IsDead.Value)
        {
            OnDeath.Raise();
            IsDead.Value = true;
        }
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

    public void OnAntiDarkness(Component sender, object data)
    {
        var time = (float)data;
        _antiDarknessTime = time;
        ADText.text = ((int)time).ToString();
        ADImage.enabled = true;
        ADBack.enabled = true;
        ADText.enabled = true;
    }

    private void HandleAntiDarkness()
    {
        if (_antiDarknessTime <= 0)
        {
            ADImage.enabled = false;
            ADBack.enabled = false;
            ADText.enabled = false;
        }
        else
        {
            _antiDarknessTime--;
            ADText.text = Mathf.RoundToInt(_antiDarknessTime).ToString();
        }
    }

    public void OnHeal(Component sender, object data)
    {
        Heal((int)data);
    }

    private void HandleShadow()
    {
        var isNight = IsNight.Value;
        var inSafeZone = IsInSafeZone.Value;
        var inDarkArea = IsInDarkArea.Value;

        if (isNight)
        {
            if (inSafeZone)
            {
                Heal(HealTick);
            }
            else if (inDarkArea)
            {
                if (_antiDarknessTime <= 0)
                {
                    TakeDamage(DarknessDamageTick);
                }
            }
            else
            {
                if (_antiDarknessTime <= 0)
                {
                    TakeDamage(DarknessDamageTick);
                }
            }
        }
        else if (!inDarkArea)
        {
            Heal(HealTick);
        }
        else
        {
            if (_antiDarknessTime <= 0)
            {
                TakeDamage(DarknessDamageTick);
            }
        }
    }


    public void OnTakeDamage(Component sender, object data)
    {
        TakeDamage((int)data);
    }
}