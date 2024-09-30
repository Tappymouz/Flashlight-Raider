using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCurrency : MonoBehaviour
{
    public void AddCurrency(int amount)
    {
        // Add currency and save
        PlayerManager.instance.playerData.currency += amount;
        PlayerManager.instance.SavePlayerData();

        // Update currency display
        FindObjectOfType<CurrencyDisplay>().UpdateCurrencyDisplay();
    }
}

