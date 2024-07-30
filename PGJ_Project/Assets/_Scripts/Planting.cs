using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Planting : MonoBehaviour
{
    [SerializeField] private InventoryData PlayerInventory;
    [SerializeField] private FloatVariable BambooGrowTime;
    [SerializeField] private FloatVariable BambooCount;
    [SerializeField] private GameObject[] Models;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private Item BambooItem, BambooSeed;
    [SerializeField] private Transform ItemSpawnPoint;
    [SerializeField] private GameEvent Info;
    private float _part1, _part2, _part3, _part4;
    private float _remainingTime;
    private bool _isGrowing, _ready;

    private void Start()
    {
        _part1 = BambooGrowTime.Value * 0.25f;
        _part2 = BambooGrowTime.Value * 0.5f;
        _part3 = BambooGrowTime.Value * 0.75f;
        Text.text = "[E] To Plant Bamboo Seed";
        InvokeRepeating(nameof(Grow), 1, 1);
    }

    public void OnInteract()
    {
        var hasBamboo = PlayerInventory.Inventory.TryGetValue(BambooSeed, out var bambooSeedCount); 
        if (!_isGrowing && !_ready)
        {
            if (hasBamboo && bambooSeedCount > 0)
            {
                PlayerInventory.Inventory[BambooSeed]--;
                _remainingTime = BambooGrowTime.Value;
                Models[0].SetActive(true);
                _isGrowing = true;
            }
            else if (!hasBamboo || bambooSeedCount <= 0)
            {
                Info.Raise("You Don't Have Any Seeds!");
            }

        }
        else if (!_isGrowing && _ready)
        {
            StartCoroutine(nameof(SpawnItems));
            _ready = false;
            Models[3].SetActive(false);
            Text.text = "[E] To Plant Bamboo Seed";
        }
    }

    private void Grow()
    {
        if (!_isGrowing) return;
        Text.text = "";
        if (_remainingTime < _part1)
        {
            Models[2].SetActive(false);
            Models[3].SetActive(true);
            Text.text = "[E] To Harvest";
            _ready = true;
            _isGrowing = false;
        }
        else if (_remainingTime < _part2)
        {
            Models[1].SetActive(false);
            Models[2].SetActive(true);
        }
        else if (_remainingTime < _part3)
        {
            Models[0].SetActive(false);
            Models[1].SetActive(true);
        }
        _remainingTime--;
    }
    
    private IEnumerator SpawnItems()
    {
        for (var i = 0; i < (int)BambooCount.Value; i++)
        {
            Instantiate(BambooItem.ModelPrefab, ItemSpawnPoint.position , quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
