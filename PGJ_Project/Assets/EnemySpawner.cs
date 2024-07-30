using Unity.Mathematics;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject Enemy;
    [SerializeField] private ParticleSystem SpawnEffect;
    [SerializeField] private float Time;
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
            SpawnEffect.Play();
            Instantiate(Enemy, transform.position, quaternion.identity, transform);
            _elapsedTime = 0;
        }

        _elapsedTime++;
    }

    private void CheckIsSpawned()
    {
        _isSpawned = transform.childCount > 1;
    }
}