using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    [Header("Character Preview")]
    public GameObject[] playerCharacters; // Array karakter
    public int equippedWeaponId = 0; // ID senjata yang di-equip

    public void EquipWeapon(int itemId)
    {
        equippedWeaponId = itemId;
        UpdateCharacterPreview();
    }

    private void UpdateCharacterPreview()
    {
        foreach (var character in playerCharacters)
        {
            character.SetActive(false);
        }

        if (equippedWeaponId >= 0 && equippedWeaponId < playerCharacters.Length)
        {
            playerCharacters[equippedWeaponId].SetActive(true);
        }
        else
        {
            Debug.LogError("Weapon ID out of range.");
        }
    }
}
