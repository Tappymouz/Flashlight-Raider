using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public int itemId;                  // The ID of this item
    public int price;
    public Text priceText;
    public Button buyButton;
    public Button equipButton;
    private ShopItem[] allShopItems;

    [Header("Shop UI Manager Reference")]
    public ShopUIManager shopUIManager; // Hubungkan ShopUIManager

    void Start()
    {
        // Initialize the button states when the scene starts
        UpdateButtonStates();
        allShopItems = FindObjectsOfType<ShopItem>();
    }

    // Method to update button states based on purchase status
    public void UpdateButtonStates()
    {
        bool isBought = PlayerDataManager.instance.playerData.itemsBought[itemId];

        buyButton.gameObject.SetActive(!isBought);   // Show buy button if not bought
        equipButton.gameObject.SetActive(isBought);  // Show equip button if bought

        // Update equip button state
        UpdateEquipButtons();
    }

    public void BuyItem()
    {
        if (PlayerDataManager.instance.playerData.currency >= price)
        {
            // Deduct currency and mark the item as bought
            PlayerDataManager.instance.playerData.currency -= price;
            PlayerDataManager.instance.playerData.itemsBought[itemId] = true;

            // Save updated data
            PlayerDataManager.instance.SavePlayerData();

            // Update button states after purchase
            UpdateButtonStates();

            // Update the currency display
            FindObjectOfType<CurrencyDisplay>().UpdateCurrencyDisplay();
            AudioManager.Instance.PlaySFX("Click");
        }
        else
        {
            Debug.Log("Not enough currency!");
        }
    }

    public void EquipItem()
    {
        // Equip this item
        PlayerDataManager.instance.playerData.equippedCharacterId = itemId;

        // Save the equipped item data
        PlayerDataManager.instance.SavePlayerData();

        // Update all equip button states after equipping a new item
        UpdateAllEquipButtons();

        // Inform ShopUIManager to update the character preview
        shopUIManager.EquipWeapon(itemId); // Panggil fungsi di ShopUIManager

        // Debug message for equipping
        Debug.Log("Equipped item ID: " + itemId);
        AudioManager.Instance.PlaySFX("Click");
    }

    // Method to update the interactable state of all equip buttons
    public void UpdateAllEquipButtons()
    {
        foreach (ShopItem item in allShopItems)
        {
            // Check if this item is the currently equipped one
            bool isEquipped = (PlayerDataManager.instance.playerData.equippedCharacterId == item.itemId);

            // Equip button is non-interactable if the item is equipped, otherwise it's interactable
            item.equipButton.interactable = !isEquipped;
        }
    }

    // Method to ensure buttons are updated at the start
    public void UpdateEquipButtons()
    {
        // Check if this item is the currently equipped one
        bool isEquipped = (PlayerDataManager.instance.playerData.equippedCharacterId == itemId);

        // Set the interactable state of the equip button
        equipButton.interactable = !isEquipped;
    }
}
