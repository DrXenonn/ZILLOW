using UnityEngine;

public class BossCameraAnimation : MonoBehaviour
{
    [SerializeField] private BossSpawner BossSpawner;
    
    public void OnAnimationEnd()
    {
        BossSpawner.OnCameraAnimationEnd();
    }
}
