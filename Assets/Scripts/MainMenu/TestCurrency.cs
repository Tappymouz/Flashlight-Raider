using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCurrency : MonoBehaviour
{
    public static event System.Action OnCurrencyChanged;
    public void AddCurrency(int amount)
    {
        // Add currency and save
        PlayerDataManager.instance.playerData.currency += amount;
        PlayerDataManager.instance.SavePlayerData();

        // Update currency display
        FindObjectOfType<CurrencyDisplay>().UpdateCurrencyDisplay();
        OnCurrencyChanged?.Invoke();
    }
}

