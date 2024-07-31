using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent Agent;
    [SerializeField] private Animator Animator;
    [SerializeField] private bool IsBoss;
    private Transform _playerTransform;
    private bool _isInRange;

    private void Awake()
    {
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        if (!IsBoss) return;
        FollowPlayer();
    }
    
    private void Update()
    {
        if (!_isInRange) return;

        Agent.SetDestination(_playerTransform.position);

        var direction = (_playerTransform.position - transform.position).normalized;
        direction.y = 0;
        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("EnemyTrigger")) return;
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        Animator.SetTrigger("IsRunning");
        _isInRange = true;
    }

    public void OnPlayerDeath(Component sender, object data)
    {
        if (!IsBoss)
        {
            Destroy(gameObject, 6f);
        }
    }
}