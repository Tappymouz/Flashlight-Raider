using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] roomSpawnPoints; // Array of 9 spawn points (top-left, top-center, top-right, etc.)
    [SerializeField]
    private GameObject[] roomPresets; // Array of room presets
    private Dictionary<GameObject, int> presetUsageCount = new Dictionary<GameObject, int>(); // Track usage of each preset

    private void Start()
    {
        InitializePresetUsage();
        SpawnRooms();
    }

    private void InitializePresetUsage()
    {
        // Initialize the usage count for each preset to 0
        foreach (GameObject preset in roomPresets)
        {
            presetUsageCount[preset] = 0;
        }
    }

    private void SpawnRooms()
    {
        List<int> availableSpawnPoints = new List<int>();

        // Add all spawn point indices to the list
        for (int i = 0; i < roomSpawnPoints.Length; i++)
        {
            availableSpawnPoints.Add(i);
        }

        // Randomly assign room presets to each spawn point
        foreach (Transform spawnPoint in roomSpawnPoints)
        {
            GameObject selectedPreset = GetRandomPreset();

            if (selectedPreset != null)
            {
                // Instantiate the preset at the spawn point and set it as a child
                GameObject spawnedRoom = Instantiate(selectedPreset, spawnPoint.position, Quaternion.identity, spawnPoint);
                presetUsageCount[selectedPreset]++; // Increment the usage count for the selected preset
            }
        }
    }

    private GameObject GetRandomPreset()
    {
        List<GameObject> availablePresets = new List<GameObject>();

        // Filter presets that haven't been used more than 2 times
        foreach (GameObject preset in roomPresets)
        {
            if (presetUsageCount[preset] < 2) // Only allow presets that have been used less than 2 times
            {
                availablePresets.Add(preset);
            }
        }

        // Return a random preset from the available ones
        if (availablePresets.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePresets.Count);
            return availablePresets[randomIndex];
        }

        return null; // If no presets are available, return null
    }
}
