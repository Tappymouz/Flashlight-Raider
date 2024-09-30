using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;   // Singleton instance
    public PlayerData playerData;           // Holds the player's data

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Make sure this persists across scenes
        }
        else
        {
            Destroy(gameObject);            // Ensure only one instance exists
        }
    }

    void Start()
    {
        // Load player data once when the game starts
        playerData = SaveSystem.LoadPlayer(3);
    }

    public void SavePlayerData()
    {
        // Save the player data whenever needed
        SaveSystem.SavePlayer(playerData);
    }
}

