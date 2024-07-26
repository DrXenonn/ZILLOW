using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    [SerializeField] private float CooldownTime;
    [SerializeField] private GameObject CooldownObject;
    [SerializeField] private Image CooldownImage;
    [SerializeField] private BoolVariable CanShoot;
    private float _timeShot;
    
    private void Update()
    {
        var timeSinceLastShot = Time.time - _timeShot;
        CanShoot.Value = CooldownTime <= timeSinceLastShot;
        CooldownImage.fillAmount = Mathf.Min(timeSinceLastShot / CooldownTime, 1f);
        
        if (CanShoot.Value)
        {
            CooldownObject.SetActive(false);
        }
    }

    public void OnShoot(Component sender, object data)
    {
        _timeShot = Time.time;
        CooldownObject.SetActive(true);
    }
}
