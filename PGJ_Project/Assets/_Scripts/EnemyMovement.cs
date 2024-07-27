using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Agent;
    private Transform _playerTransform;
    private bool _isInRange;

    private void Awake()
    {
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    
    private void Update()
    {
        if (!_isInRange) return;
        
        Agent.SetDestination(_playerTransform.position);
        transform.LookAt(_playerTransform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("EnemyTrigger")) return;
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        _isInRange = true;
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("EnemyTrigger")) return;
        _isInRange = false;
    }*/
}
