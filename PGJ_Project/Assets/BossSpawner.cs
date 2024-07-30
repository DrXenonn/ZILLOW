using System;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Hills;
    [SerializeField] private GameObject CutSceneCam;
    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    public void OnInteract()
    {
        CutSceneCam.transform.forward = _mainCam.transform.forward;
        CutSceneCam.transform.position = _mainCam.transform.position;
        Hills.SetActive(true);
        CutSceneCam.SetActive(true);
    }

    public void OnCameraAnimationEnd()
    {
        CutSceneCam.SetActive(false);
    }
}
