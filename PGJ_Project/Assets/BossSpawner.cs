using UnityEngine;
using Cinemachine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Hills;
    [SerializeField] private GameObject CutSceneCam;
    [SerializeField] private Transform BossParent;
    [SerializeField] private BoolVariable CanMove;
    [SerializeField] private CinemachineVirtualCamera MainCam;
    [SerializeField] private Transform Player;
    private bool _isPlayingCutScene;

    public void OnInteract()
    {
        if (_isPlayingCutScene) return;
        _isPlayingCutScene = true;
        CutSceneCam.transform.rotation = MainCam.transform.rotation;
        CutSceneCam.transform.position = MainCam.transform.position;
        Hills.SetActive(true);
        CutSceneCam.SetActive(true);
        CanMove.Value = false;
    }
    
    public void OnCameraAnimationEnd()
    {
        _isPlayingCutScene = false;
        CanMove.Value = true;
        CutSceneCam.SetActive(false);

        LookAtBoss();
    }
    
    private void LookAtBoss()
    {
        var directionToBoss = BossParent.position - Player.position;
        directionToBoss.y = 0;
        var lookRotation = Quaternion.LookRotation(directionToBoss);
        Player.rotation = lookRotation;
        var pov = MainCam.GetCinemachineComponent<CinemachinePOV>();
        pov.m_HorizontalAxis.Value = -180;
    }
}
