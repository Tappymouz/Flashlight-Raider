using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    public TextMeshProUGUI currencyText;

    private void Start()
    {
        UpdateCurrencyDisplay();
    }
    private void OnEnable()
    {
        // Subscribe to the event when the game starts
        TestCurrency.OnCurrencyChanged += UpdateCurrencyDisplay;
        UpdateCurrencyDisplay(); // Ensure the text is updated when the UI element is enabled
    }

    private void OnDisable()
    {
        // Unsubscribe when the game object is disabled to prevent memory leaks
        TestCurrency.OnCurrencyChanged -= UpdateCurrencyDisplay;
    }

    public void UpdateCurrencyDisplay()
    {
        // Get currency from the central PlayerManager
        currencyText.text = PlayerDataManager.instance.playerData.currency.ToString();
    }
}

