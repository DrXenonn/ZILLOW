using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private GameEvent OnAttack;
    [SerializeField] private Animator Animator;
    [SerializeField] private int Damage;
    private bool _isClose;
    
    public void Attack()
    {
        if (!_isClose) return;
        OnAttack.Raise(Damage);
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
