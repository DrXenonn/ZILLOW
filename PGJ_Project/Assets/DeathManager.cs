using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private InventoryData PlayerInventory;
    [SerializeField] private GameEvent OnHeal;
    [SerializeField] private GameEvent OnItemPickup;
    [SerializeField] private BoolVariable IsDead;
    [SerializeField] private Transform PlayerT, SpawnPoint;
    [SerializeField] private BoolVariable CanMove;
    [SerializeField] private GameObject DeathScreen;
    [SerializeField] private TextMeshProUGUI DeathTimer;
    [SerializeField] private PotionUsing PotionUsing;
    [SerializeField] private Crossbow Crossbow;
    [SerializeField] private BoolVariable IsInSafeZone, IsInDarkArea;
    [SerializeField] private float DeathScreenTimer;
    [SerializeField] private float PercentageToKeep;
    [SerializeField] private int MinItemCountToLose;
    

    public void OnDeath(Component sender, object data)
    {
        CanMove.Value = false;
        DeathScreen.SetActive(true);
        StartCoroutine(nameof(HandleDeathTimer));
        HandleInventory();
        
        Invoke(nameof(OnRespawn), DeathScreenTimer);
    }

    private void OnRespawn()
    {
        OnHeal.Raise(999999);
        DeathScreen.SetActive(false);
        IsDead.Value = false;
        PlayerT.position = SpawnPoint.position;
        CanMove.Value = true;
        PotionUsing.UpdateUI();
        Crossbow.UpdateUI();
        IsInDarkArea.Value = false;
        IsInSafeZone.Value = true;
    }

    private IEnumerator HandleDeathTimer()
    {
        var seconds = DeathScreenTimer;
        while (seconds >= 1)
        {
            DeathTimer.text = $"You will be respawned in {seconds} seconds";
            yield return new WaitForSeconds(1);
            seconds--;
        }
    }

    private void HandleInventory()
    {
        var itemsToUpdate = new Dictionary<Item, int>();

        foreach (var keyValuePair in PlayerInventory.Inventory)
        {
            var item = keyValuePair.Key;
            var itemCount = keyValuePair.Value;

            if (itemCount >= MinItemCountToLose)
            {
                var newCount = Mathf.CeilToInt(itemCount * PercentageToKeep);
                itemsToUpdate[item] = newCount;
            }
        }

        foreach (var kvp in itemsToUpdate)
        {
            PlayerInventory.Inventory[kvp.Key] = kvp.Value;
        }
    }
}