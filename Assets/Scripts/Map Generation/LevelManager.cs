using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class RoomPrefabs
    {
        public List<GameObject> topLeftRooms;
        public List<GameObject> topRightRooms;
        public List<GameObject> bottomLeftRooms;
        public List<GameObject> bottomRightRooms;
    }

    public RoomPrefabs roomPrefabs;

    public Transform topLeftSpawn;
    public Transform topRightSpawn;
    public Transform bottomLeftSpawn;
    public Transform bottomRightSpawn;

    void Start()
    {
        SpawnRooms();
    }

    void SpawnRooms()
    {
        // Spawn a room in the top-left corner
        if (roomPrefabs.topLeftRooms.Count > 0)
        {
            GameObject room = Instantiate(roomPrefabs.topLeftRooms[Random.Range(0, roomPrefabs.topLeftRooms.Count)], topLeftSpawn);
            room.transform.localPosition = Vector2.zero; // Set the position relative to the spawn point
        }

        // Spawn a room in the top-right corner
        if (roomPrefabs.topRightRooms.Count > 0)
        {
            GameObject room = Instantiate(roomPrefabs.topRightRooms[Random.Range(0, roomPrefabs.topRightRooms.Count)], topRightSpawn);
            room.transform.localPosition = Vector2.zero; // Set the position relative to the spawn point
        }

        // Spawn a room in the bottom-left corner
        if (roomPrefabs.bottomLeftRooms.Count > 0)
        {
            GameObject room = Instantiate(roomPrefabs.bottomLeftRooms[Random.Range(0, roomPrefabs.bottomLeftRooms.Count)], bottomLeftSpawn);
            room.transform.localPosition = Vector2.zero; // Set the position relative to the spawn point
        }

        // Spawn a room in the bottom-right corner
        if (roomPrefabs.bottomRightRooms.Count > 0)
        {
            GameObject room = Instantiate(roomPrefabs.bottomRightRooms[Random.Range(0, roomPrefabs.bottomRightRooms.Count)], bottomRightSpawn);
            room.transform.localPosition = Vector2.zero; // Set the position relative to the spawn point
        }
    }

}