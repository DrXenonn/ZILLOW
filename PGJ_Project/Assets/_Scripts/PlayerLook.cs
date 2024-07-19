using Cinemachine;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private FloatVariable Sens;
    [SerializeField] private CinemachineVirtualCamera Cinemachine;
    private CinemachinePOV _pov;

    private void Awake()
    {
        _pov = Cinemachine.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        UpdateSens();
    }

    public void UpdateSens() //Might use this later for settings.
    {
        _pov.m_HorizontalAxis.m_MaxSpeed = Sens.Value;
        _pov.m_VerticalAxis.m_MaxSpeed = Sens.Value;
    }
}