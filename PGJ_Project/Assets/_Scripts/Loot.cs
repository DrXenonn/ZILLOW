using TMPro;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private Item ItemDrop;
    [SerializeField] private GameObject ReadyObject, NotReadyObject;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private float Time;
    private bool _isGrowing;
    private float _elapsedTime;

    private void Start()
    {
        InvokeRepeating(nameof(Grow), 1, 1);
    }

    public void OnInteract()
    {
        if (_isGrowing) return;
        Text.text = "";
        _isGrowing = true;
        NotReadyObject.SetActive(true);
        ReadyObject.SetActive(false);
        Instantiate(ItemDrop.ModelPrefab, SpawnPoint.position, Quaternion.identity);
    }

    private void Grow()
    {
        if (!_isGrowing) return;
        _elapsedTime++;
        
        if (_elapsedTime >= Time)
        {
            ReadyObject.SetActive(true);
            NotReadyObject.SetActive(false);
            Text.text = "[E] To Gather";
            _elapsedTime = 0;
            _isGrowing = false;
        }
    }
}