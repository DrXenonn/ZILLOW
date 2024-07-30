using Unity.Mathematics;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject Enemy;
    [SerializeField] private ParticleSystem SpawnEffect;
    [SerializeField] private BoolVariable IsNight;
    [SerializeField] private float Time;
    [SerializeField] private bool OnlySpawnAtNight;
    private float _elapsedTime;
    private bool _isSpawned = true;

    private void Start()
    {
        InvokeRepeating(nameof(HandleSpawn), 1, 1);
    }

    private void HandleSpawn()
    {
        CheckIsSpawned();
        if (_isSpawned) return;

        if (!_isSpawned && _elapsedTime >= Time)
        {
            if (OnlySpawnAtNight && IsNight.Value)
            {
                Spawn();
            }
            else if (!OnlySpawnAtNight)
            {
                Spawn();
            }
        }

        _elapsedTime++;
    }

    private void Spawn()
    {
        SpawnEffect.Play();
        Instantiate(Enemy, transform.position, quaternion.identity, transform);
        _elapsedTime = 0;
    }

    private void CheckIsSpawned()
    {
        _isSpawned = transform.childCount > 1;
    }
}