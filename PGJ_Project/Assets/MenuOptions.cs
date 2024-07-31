using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
    [SerializeField] private FloatVariable MouseSens;
    [SerializeField] private AudioMixer Music, SoundEffect;
    [SerializeField] private FloatVariable MusicVolume, SoundEffectVolume;
    [SerializeField] private Slider MusicSlider, SfxSlider, SensSlider;
    [SerializeField] private FloatVariable MusicSliderValue, SfxSliderValue, MouseSensSliderValue;

    private void Start()
    {
        Music.SetFloat("Volume", MusicVolume.Value);
        SoundEffect.SetFloat("sVolume", SoundEffectVolume.Value);
    }
    
    public void OnPlay()
    {
        StartCoroutine(LoadSceneAsyncAdditive(1));
    }

    private IEnumerator LoadSceneAsyncAdditive(int sceneIndex)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }


    public void SetMusicVolume(float volume)
    {
        var clampedVolume = (volume == 0f) ? -80f : Mathf.Clamp(volume * 30f, -30f, 15f);
        Music.SetFloat("Volume", clampedVolume);
        MusicVolume.Value = clampedVolume;
        MusicSliderValue.Value = MusicSlider.value;
    }
    
    public void SetSfxVolume(float volume)
    {
        var clampedVolume = (volume == 0f) ? -80f : Mathf.Clamp(volume * 30f, -30f, 10f);
        SoundEffect.SetFloat("sVolume", clampedVolume);
        SoundEffectVolume.Value = clampedVolume;
        SfxSliderValue.Value = SfxSlider.value;
    }

    public void SetSens(float value)
    {
        var clampedValue = MapToRange(value, 0.005f, 2.5f);
        MouseSens.Value = clampedValue;
        MouseSensSliderValue.Value = SensSlider.value;
    }

    private void UpdateSettingSliders()
    {
        MusicSlider.value = MusicSliderValue.Value;
        SfxSlider.value = SfxSliderValue.Value;
        SensSlider.value = MouseSensSliderValue.Value;
    }
    
    private float MapToRange(float value, float min, float max)
    {
        value = Mathf.Clamp01(value);
        var newValue = min + (max - min) * value;
        return newValue;
    }
}
