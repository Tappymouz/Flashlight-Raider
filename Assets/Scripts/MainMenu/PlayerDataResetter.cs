using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataResetter : MonoBehaviour
{
    public ShopItem[] shopItems;  // Reference to all shop items in the scene

    public void ResetPlayerData()
    {
        // Reset the player's data to default values, passing the required 'itemCount'
        PlayerData defaultData = new PlayerData(4);

        // Set default values for currency and equipped item
        defaultData.currency = 0;
        defaultData.itemsBought = new bool[4];  // Reset buy status for each item
        defaultData.equippedCharacterId = 0;    // Default equipped character is item ID 0

        // Mark the default item as bought
        defaultData.itemsBought[0] = true; // Default item is considered bought

        // Save the default player data
        SaveSystem.SavePlayer(defaultData);

        // Update the in-game player data
        PlayerDataManager.instance.playerData = defaultData;

        // Update the currency display
        FindObjectOfType<CurrencyDisplay>().UpdateCurrencyDisplay();

        // Refresh the state of the shop buttons
        RefreshShopButtons();

        Debug.Log("Player data reset!");
    }

    // Method to refresh the Buy and Equip buttons
    public void RefreshShopButtons()
    {
        foreach (ShopItem item in shopItems)
        {
            // Update each shop item buttons after resetting the player data
            item.UpdateButtonStates();
        }
    }

}




