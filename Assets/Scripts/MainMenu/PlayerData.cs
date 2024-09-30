using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currency;
    public bool[] itemsBought;
    public int equippedCharacterId; // Corresponds to a character in your game

    public PlayerData(int itemCount)
    {
        currency = 0;
        itemsBought = new bool[itemCount];
        equippedCharacterId = 0; // Default equipped character (ID 0)
    }
}

