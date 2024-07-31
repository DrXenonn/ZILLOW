using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource MainSource;
    [SerializeField] private AudioSource DarkAreaSource;
    [SerializeField] private AudioClip[] MusicTracks;
    [SerializeField] private AudioClip DarkAreaMusic;
    [SerializeField] private float FadeDuration;
    [SerializeField] private float DarkAreaVolume;
    [SerializeField] private float MainTrackLowVolume;
    [SerializeField] private float MainMusicMaxVolume;

    private int _currentTrackIndex;
    private bool _isInDarkArea;

    private void Start()
    {
        DarkAreaSource.loop = true;
        DarkAreaSource.clip = DarkAreaMusic;
        PlayNextTrack();
        InvokeRepeating(nameof(SlowUpdate), 1f, 1f);
    }

    private void SlowUpdate()
    {
        if (!MainSource.isPlaying && !_isInDarkArea)
        {
            PlayNextTrack();
        }
    }

    private void PlayNextTrack()
    {
        if (MusicTracks.Length == 0) return;

        MainSource.clip = MusicTracks[_currentTrackIndex];
        MainSource.Play();
        _currentTrackIndex = (_currentTrackIndex + 1) % MusicTracks.Length;
    }

    private IEnumerator FadeVolume(AudioSource audioSource, float targetVolume, float duration)
    {
        var startVolume = audioSource.volume;
        var elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void OnEnterDarkArea(Component sender, object data)
    {
        _isInDarkArea = true;
        StartCoroutine(FadeVolume(MainSource, MainTrackLowVolume, FadeDuration));
        StartCoroutine(FadeVolume(DarkAreaSource, DarkAreaVolume, FadeDuration));
        if (!DarkAreaSource.isPlaying)
        {
            DarkAreaSource.Play();
        }
    }

    public void OnExitDarkArea(Component sender, object data)
    {
        _isInDarkArea = false;
        StartCoroutine(FadeVolume(MainSource, MainMusicMaxVolume, FadeDuration));
        StartCoroutine(FadeVolume(DarkAreaSource, 0f, FadeDuration));
    }
}
