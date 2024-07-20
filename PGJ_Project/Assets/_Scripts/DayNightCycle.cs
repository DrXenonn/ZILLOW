using UnityEngine;

public class SkyboxBlender : MonoBehaviour
{
    [SerializeField] private float TransitionTime;
    [SerializeField] private Material DayMaterial;
    [SerializeField] private Material NightMaterial;
    [SerializeField] private Material BlendMaterial;
    [SerializeField] private float TransitionDuration = 3f;
    [SerializeField] private Light GlobalLight;
    [SerializeField] private float TargetLightIntensity;
    [SerializeField] private float TargetTemperature;
    [SerializeField] private Vector3 TargetRotation;
    [SerializeField] private float AmbientIntensity;
    [SerializeField] private float ReflectionsIntensity;
    [SerializeField] private float ShadowStrength;
    
    private Material _targetMaterial;
    private float _transitionTimer;
    private bool _isTransitioning;

    private float _startingLightIntensity;
    private float _startingTemperature;
    private Quaternion _startingRotation;
    private bool _isDay = true;
    private float _skyboxTransitionValue;
    
    private const string TintColor = "_TintColor";
    private const string Exposure = "_Exposure";
    private const string FogIntensity = "_FogIntensity";
    private const string FogHeight = "_FogHeight";
    private const string FogSmoothness = "_FogSmoothness";
    private const string FogFill = "_FogFill";
    private const string CubemapTransition = "_CubemapTransition";
    
    private static readonly int Transition = Shader.PropertyToID(CubemapTransition);
    private static readonly int TintColor1 = Shader.PropertyToID(TintColor);
    private static readonly int Exposure1 = Shader.PropertyToID(Exposure);
    private static readonly int Intensity = Shader.PropertyToID(FogIntensity);
    private static readonly int Height = Shader.PropertyToID(FogHeight);
    private static readonly int Smoothness = Shader.PropertyToID(FogSmoothness);
    private static readonly int Fill = Shader.PropertyToID(FogFill);
    
    private void Start()
    {
        _startingLightIntensity = GlobalLight.intensity;
        _startingTemperature = GlobalLight.colorTemperature;
        _startingRotation = GlobalLight.transform.rotation;

        _targetMaterial = new Material(BlendMaterial);

        CopyMaterialProperties(DayMaterial, _targetMaterial);

        RenderSettings.skybox = _targetMaterial;

        InvokeRepeating(nameof(StartTransitionNoParam), TransitionTime, TransitionTime + TransitionDuration);

        InvokeRepeating(nameof(UpdateScript), TransitionTime, 0.1f);
    }

    private void StartTransitionNoParam()
    {
        if (_isDay)
        {
            StartTransition(DayMaterial);
        }
        else
        {
            StartTransition(NightMaterial);
        }
    }

    private void Update()
    {
        GlobalLight.transform.Rotate(Vector3.right, 180 * Time.deltaTime / (TransitionTime + TransitionDuration));
    }

    private void UpdateScript()
    {
        //TODO: Globallight should rotate constantly.
        if (!_isTransitioning) return;
        
        var progress = Mathf.Clamp01(_transitionTimer / TransitionDuration);
        _transitionTimer += 0.1f;

        if (_isDay)
        {
            LerpMaterialProperties(DayMaterial, NightMaterial, _targetMaterial, progress);

            GlobalLight.intensity = Mathf.Lerp(_startingLightIntensity, TargetLightIntensity, progress);
            GlobalLight.colorTemperature = Mathf.Lerp(_startingTemperature, TargetTemperature, progress);
            RenderSettings.ambientIntensity = Mathf.Lerp(1, AmbientIntensity, progress);
            RenderSettings.reflectionIntensity = Mathf.Lerp(1, ReflectionsIntensity, progress);
            GlobalLight.shadowStrength = Mathf.Lerp(1, ShadowStrength, progress);
            if (!(_skyboxTransitionValue >= 1)) return;
            _isDay = false;
            _isTransitioning = false;
        }
        else
        {
            LerpMaterialProperties(NightMaterial, DayMaterial, _targetMaterial, progress);

            GlobalLight.intensity = Mathf.Lerp(TargetLightIntensity, _startingLightIntensity, progress);
            GlobalLight.colorTemperature = Mathf.Lerp(TargetTemperature, _startingTemperature, progress);
            RenderSettings.ambientIntensity = Mathf.Lerp(AmbientIntensity, 1, progress);
            RenderSettings.reflectionIntensity = Mathf.Lerp(ReflectionsIntensity, 1, progress);
            GlobalLight.shadowStrength = Mathf.Lerp(ShadowStrength, 1, progress);

            if (!(_skyboxTransitionValue <= 0)) return;
            _isDay = true;
            _isTransitioning = false;
        }
    }
    
    private void StartTransition(Material startMaterial)
    {
        _transitionTimer = 0f;

        BlendMaterial.SetFloat(Transition, 0f);

        CopyMaterialProperties(startMaterial, BlendMaterial);

        _isTransitioning = true;
    }

    private void LerpMaterialProperties(Material startMaterial, Material endMaterial, Material targetMaterial, float t)
    {
        targetMaterial.SetColor(TintColor1,
            Color.Lerp(startMaterial.GetColor(TintColor1), endMaterial.GetColor(TintColor1), t));
        targetMaterial.SetFloat(Exposure1,
            Mathf.Lerp(startMaterial.GetFloat(Exposure1), endMaterial.GetFloat(Exposure1), t));
        targetMaterial.SetFloat(Intensity,
            Mathf.Lerp(startMaterial.GetFloat(Intensity), endMaterial.GetFloat(Intensity), t));
        targetMaterial.SetFloat(Height,
            Mathf.Lerp(startMaterial.GetFloat(Height), endMaterial.GetFloat(Height), t));
        targetMaterial.SetFloat(Smoothness,
            Mathf.Lerp(startMaterial.GetFloat(Smoothness), endMaterial.GetFloat(Smoothness), t));
        targetMaterial.SetFloat(Fill, Mathf.Lerp(startMaterial.GetFloat(Fill), endMaterial.GetFloat(Fill), t));
        if (_isDay)
        {
            var value = Mathf.Lerp(0f, 1f, t);
            targetMaterial.SetFloat(Transition, value);
            _skyboxTransitionValue = value;
        }
        else
        {
            var value = Mathf.Lerp(1f, 0f, t);
            targetMaterial.SetFloat(Transition, value);
            _skyboxTransitionValue = value;
        }
    }

    private void CopyMaterialProperties(Material sourceMaterial, Material targetMaterial)
    {
        targetMaterial.SetColor(TintColor1, sourceMaterial.GetColor(TintColor1));
        targetMaterial.SetFloat(Exposure1, sourceMaterial.GetFloat(Exposure1));
        targetMaterial.SetFloat(Intensity, sourceMaterial.GetFloat(Intensity));
        targetMaterial.SetFloat(Height, sourceMaterial.GetFloat(Height));
        targetMaterial.SetFloat(Smoothness, sourceMaterial.GetFloat(Smoothness));
        targetMaterial.SetFloat(Fill, sourceMaterial.GetFloat(Fill));
    }
}
