using System;
using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private GameEvent OnAttack;
    [SerializeField] private Animator Animator;
    [SerializeField] private EnemyMovement Movement;
    [SerializeField] private int Damage;
    private bool _isClose;
    private float _originalSpeed;

    private void Start()
    {
        _originalSpeed = Movement.Agent.speed;
    }

    public void Attack()
    {
        if (!_isClose) return;
        OnAttack.Raise(Damage);
        StopAllCoroutines();
        StartCoroutine(nameof(HandleMoveSpeedChange));
    }

    private IEnumerator HandleMoveSpeedChange()
    {
        Movement.Agent.speed /= 1.7f;
        yield return new WaitForSeconds(0.6f);
        Movement.Agent.speed = _originalSpeed / 1.4f;
        yield return new WaitForSeconds(0.6f);
        Movement.Agent.speed = _originalSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("EnemyAttackRange")) return;
        Animator.SetBool("IsCloseToPlayer", true);
        _isClose = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("EnemyAttackRange")) return;
        Animator.SetBool("IsCloseToPlayer", false);
        _isClose = false;
    }
}
