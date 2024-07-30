using System.Collections;
using UnityEngine;
using Cinemachine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject CutSceneCam;
    [SerializeField] private Transform BossParent;
    [SerializeField] private ParticleSystem BossSpawnParticles;
    [SerializeField] private GameObject BossPrefab;
    [SerializeField] private GameObject Hills;
    [SerializeField] private Transform HillsSpawnPoint;
    [SerializeField] private BoolVariable CanMove;
    [SerializeField] private CinemachineVirtualCamera MainCam;
    [SerializeField] private Transform Player;
    [SerializeField] private Light MainLight, BossLight;
    [SerializeField] private Light[] HouseLights;
    [SerializeField] private SphereCollider EnemyTriggerRange;
    [SerializeField] private float BossSpawnTime;
    private bool _isPlayingCutScene;
    private GameObject _hills;
    
    public void OnInteract()
    {
        if (_isPlayingCutScene) return;
        _isPlayingCutScene = true;
        CutSceneCam.transform.rotation = MainCam.transform.rotation;
        CutSceneCam.transform.position = MainCam.transform.position;
        CutSceneCam.SetActive(true);
        CanMove.Value = false;
        HandleLight();
        _hills = Instantiate(Hills, HillsSpawnPoint);
        _hills.GetComponent<Animator>().Play("BossHills");
        Invoke(nameof(PlayParticles), 5f);
        Invoke(nameof(SpawnBoss), BossSpawnTime);
        EnemyTriggerRange.enabled = false;
    }

    private void PlayParticles()
    {
        BossSpawnParticles.Play();
        Invoke(nameof(StopParticles), 6f);
    }

    private void StopParticles()
    {
        BossSpawnParticles.Stop();
    }


    private void SpawnBoss()
    {
        Instantiate(BossPrefab, BossParent);
    }

    public void OnCameraAnimationEnd()
    {
        _isPlayingCutScene = false;
        CanMove.Value = true;
        CutSceneCam.SetActive(false);

        LookAtBoss();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnDeath(null, null);
        }
    }

    private void HandleLight()
    {
        StartCoroutine(nameof(LightTransition));
    }

    private IEnumerator LightTransition()
    {
        BossLight.intensity = 0;
        foreach (var l in HouseLights)
        {
            l.enabled = false;
        }
        
        BossLight.enabled = true;
        const float intensity = 1.7f;
        
        while (BossLight.intensity < intensity)
        {
            BossLight.intensity += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        
        MainLight.enabled = false;
        BossLight.intensity = intensity;
    }

    private void LookAtBoss()
    {
        var directionToBoss = BossParent.position - Player.position;
        directionToBoss.y = 0;
        var lookRotation = Quaternion.LookRotation(directionToBoss);
        Player.rotation = lookRotation;
        var pov = MainCam.GetCinemachineComponent<CinemachinePOV>();
        pov.m_HorizontalAxis.Value = Player.rotation.y;
    }

    public void OnDeath(Component sender, object data)
    {
        foreach (var l in HouseLights)
        {
            l.enabled = true;
        }

        BossLight.enabled = false;
        MainLight.enabled = true;
        EnemyTriggerRange.enabled = true;
        Destroy(_hills);
    }

}
