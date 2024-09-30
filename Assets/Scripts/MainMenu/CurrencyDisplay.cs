using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    public TextMeshProUGUI currencyText;

    void Start()
    {
        UpdateCurrencyDisplay();  // Initial display update
    }

    public void UpdateCurrencyDisplay()
    {
        // Get currency from the central PlayerManager
        currencyText.text = "Currency: " + PlayerManager.instance.playerData.currency.ToString();
    }
}

