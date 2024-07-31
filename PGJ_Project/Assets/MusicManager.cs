using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource MainSource;
    [SerializeField] private AudioSource DarkAreaSource;
    [SerializeField] private AudioSource BossFightSource;
    [SerializeField] private AudioSource CreditsSource;
    [SerializeField] private AudioClip[] MusicTracks;
    [SerializeField] private AudioClip DarkAreaMusic;
    [SerializeField] private AudioClip BossFightMusic;
    [SerializeField] private AudioClip CreditsMusic;
    [SerializeField] private float FadeDuration;
    [SerializeField] private float DarkAreaVolume;
    [SerializeField] private float MainTrackLowVolume;
    [SerializeField] private float MainMusicMaxVolume;
    [SerializeField] private float BossFightVolume;
    [SerializeField] private float CreditsVolume;

    private int _currentTrackIndex;
    private bool _isInDarkArea;
    private bool _isInBossFight;
    private bool _isCreditsPlaying;

    private void Start()
    {
        DarkAreaSource.loop = true;
        DarkAreaSource.clip = DarkAreaMusic;
        BossFightSource.loop = true;
        BossFightSource.clip = BossFightMusic;
        CreditsSource.loop = true;
        CreditsSource.clip = CreditsMusic;

        PlayNextTrack();
        InvokeRepeating(nameof(SlowUpdate), 1f, 1f);
    }

    private void SlowUpdate()
    {
        if (!MainSource.isPlaying && !_isInDarkArea && !_isInBossFight && !_isCreditsPlaying)
        {
            PlayNextTrack();
        }
    }

    private void PlayNextTrack()
    {
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

    public void OnBossFightEvent(Component sender, object data)
    {
        _isInBossFight = true;
        StartCoroutine(FadeVolume(MainSource, 0, FadeDuration));
        StartCoroutine(FadeVolume(DarkAreaSource, 0f, FadeDuration));
        StartCoroutine(FadeVolume(BossFightSource, BossFightVolume, FadeDuration));
        if (BossFightSource.isPlaying)
        {
            BossFightSource.Stop();
        }
        BossFightSource.Play();
    }

    public void OnPlayerDeath(Component sender, object data)
    {
        _isInBossFight = false;
        StartCoroutine(FadeVolume(BossFightSource, 0f, FadeDuration));
        StartCoroutine(FadeVolume(MainSource, MainMusicMaxVolume, FadeDuration + 5));
    }

    public void OnWinGame(Component sender, object data)
    {
        _isInBossFight = false;
        _isCreditsPlaying = true;
        StartCoroutine(FadeVolume(MainSource, 0f, FadeDuration + 2));
        StartCoroutine(FadeVolume(BossFightSource, 0f, FadeDuration + 5));
        Invoke(nameof(StartCoroutineLate), 2f);
        if (!CreditsSource.isPlaying)
        {
            CreditsSource.Play();
        }
    }

    private void StartCoroutineLate()
    {
        StartCoroutine(FadeVolume(CreditsSource, CreditsVolume, FadeDuration + 5));
    }
}
