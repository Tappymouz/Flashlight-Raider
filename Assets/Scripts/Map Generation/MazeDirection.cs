using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeDirection
{
    North,
    East,
    South,
    West
}

public static class MazeDirections
{
    public const int Count = 4;

    private static Vector2Int[] vectors = {
        new Vector2Int(0, 1),   // North
        new Vector2Int(-1, 0),   // East
        new Vector2Int(0, -1),  // South
        new Vector2Int(1, 0)   // West
    };

    private static MazeDirection[] opposites = {
        MazeDirection.South,  // North's opposite
        MazeDirection.West,   // East's opposite
        MazeDirection.North,  // South's opposite
        MazeDirection.East    // West's opposite
    };

    private static Quaternion[] rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(0f, 0f, 180f),
        Quaternion.Euler(0f, 0f, 270f)
    };

    public static MazeDirection RandomValue
    {
        get
        {
            return (MazeDirection)Random.Range(0, Count);
        }
    }

    public static Vector2Int ToVector2Int(this MazeDirection direction)
    {
        return vectors[(int)direction];
    }

    public static MazeDirection GetOpposite(this MazeDirection direction)
    {
        return opposites[(int)direction];
    }

    public static Quaternion ToRotation(this MazeDirection direction)
    {
        return rotations[(int)direction];
    }
}
