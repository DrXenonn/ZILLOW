using Cinemachine;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    [SerializeField] private GameObject MainCam, WinCam;
    public void OnWin(Component sender, object data)
    {
        MainCam.SetActive(false);
        WinCam.SetActive(true);
    }
}
