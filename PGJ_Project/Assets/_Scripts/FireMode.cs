using UnityEngine;
using UnityEngine.UI;

public class FireMode : MonoBehaviour
{
    [SerializeField] private Image Crosshair;
    [SerializeField] private Sprite PotionCrosshair, XBowCrosshair;
    [SerializeField] private BoolVariable XBowMode;
    [SerializeField] private GameObject PotionSelectionMenu, ArrowSelectionMenu;
    [SerializeField] private AudioSource AudioSource;

    private void Start()
    {
        XBowMode.Value = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            XBowMode.Value = !XBowMode.Value;
            SetCrosshair();
            ShowSelectionMenu();
            AudioSource.PlayOneShot(AudioSource.clip);
        }
    }

    private void SetCrosshair()
    {
        Crosshair.sprite = XBowMode.Value ? XBowCrosshair : PotionCrosshair;
    }

    private void ShowSelectionMenu()
    {
        PotionSelectionMenu.SetActive(!XBowMode.Value);
        ArrowSelectionMenu.SetActive(XBowMode.Value);
    }
}