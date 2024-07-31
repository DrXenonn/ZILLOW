using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class BossMechanics : MonoBehaviour
{
    [SerializeField] private EnemyHealth EnemyHealth;
    [SerializeField] private EnemyMovement Movement;
    [SerializeField] private Animator Animator;
    [SerializeField] private GameObject WebPrefab;
    [SerializeField] private Transform WebSpawnPoint;
    [SerializeField] private float SpitMinimumDistance;
    [SerializeField] private float SpitInterval;
    [SerializeField] private float WebForce;
    private Transform _playerT;
    private NavMeshAgent _agent;
    private float _defaultSpeed;

    private void Awake()
    {
        _playerT = GameObject.FindWithTag("Player").transform;
        _agent = Movement.Agent;
        _defaultSpeed = _agent.speed;
    }

    private void Start()
    {
        InvokeRepeating(nameof(Spit), SpitInterval, SpitInterval);
    }

    private void Spit()
    {
        if (!(Vector3.Distance(transform.position, _playerT.position) >= SpitMinimumDistance)) return;
        transform.LookAt(_playerT);
        _agent.speed = 0f;
        Animator.Play("Spit");
    }

    public void SpawnWeb()
    {
        _agent.speed = _defaultSpeed;
        var dir = (_playerT.position + Vector3.up - WebSpawnPoint.position).normalized;
        var web = Instantiate(WebPrefab, WebSpawnPoint.position, quaternion.identity);
        web.GetComponent<Rigidbody>().AddForce(dir * WebForce);
    }
}
