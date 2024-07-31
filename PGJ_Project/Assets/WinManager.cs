using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class WinManager : MonoBehaviour
{
    [SerializeField] private GameObject MainCam, WinCam;
    [SerializeField] private GameObject[] ObjectsToDisable;
    [SerializeField] private GameObject Alter;
    [SerializeField] private Camera Vc;
    
    public void OnWin(Component sender, object data)
    {
        var brain = Vc.GetComponent<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = 0;
        Alter.SetActive(true);
        Invoke(nameof(ActiveLate), 2);
    }

    private void ActiveLate()
    {
        foreach (var o in ObjectsToDisable)
        {
            o.SetActive(false);
        }
        MainCam.SetActive(false);
        WinCam.SetActive(true);
    }
}
