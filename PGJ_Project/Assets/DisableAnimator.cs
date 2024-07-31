using UnityEngine;
using UnityEngine.UI;

public class DisableAnimator : MonoBehaviour
{
    [SerializeField] private Animator DeathBarAnimation;
    [SerializeField] private Image Fill;

    public void DisableDeathBarAnimation()
    {
        Fill.fillAmount = 1f;
        DeathBarAnimation.enabled = false;
    }
}
