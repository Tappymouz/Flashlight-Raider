using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    [Header("Character Preview")]
    public GameObject[] playerCharacters; // Array karakter
    public int equippedWeaponId; // ID senjata yang di-equip

    private void Start()
    {
        UpdateCharacterPreview();
    }
    public void EquipWeapon(int itemId)
    {
        equippedWeaponId = itemId;
        UpdateCharacterPreview();
    }

    public void UpdateCharacterPreview()
    {
        // Retrieve the equipped character ID from PlayerDataManager's playerData
        int equippedId = PlayerDataManager.instance.playerData.equippedCharacterId;

        // Deactivate all character previews
        foreach (var character in playerCharacters)
        {
            character.SetActive(false);
        }

        // Activate the preview for the equipped character
        if (equippedId >= 0 && equippedId < playerCharacters.Length)
        {
            playerCharacters[equippedId].SetActive(true);
        }
        else
        {
            Debug.LogError("Equipped character ID is out of range.");
        }
    }
}

