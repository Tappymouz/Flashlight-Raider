using UnityEngine;

public class MazeCell : MonoBehaviour 
{
    
    public Vector2Int coordinates;
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
    private int initializedEdgeCount;
    public MazeRoom room;

    public bool IsFullyInitialized
    {
        get
        {
            return initializedEdgeCount == MazeDirections.Count;
        }
    }

    public MazeCellEdge GetEdge(MazeDirection direction)
    {
        return edges[(int)direction];
    }

    public void SetEdge(MazeDirection direction, MazeCellEdge edge)
    {
        edges[(int)direction] = edge;
        initializedEdgeCount += 1;
    }

    public MazeDirection RandomUninitializedDirection
    {
        get
        {
            int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
            for (int i = 0; i < MazeDirections.Count; i++)
            {
                if (edges[i] == null)
                {
                    if (skips == 0)
                    {
                        Debug.Log("Cell at " + coordinates + " choosing uninitialized direction: " + (MazeDirection)i);
                        return (MazeDirection)i;
                    }
                    skips -= 1;
                }
            }
            Debug.LogWarning("MazeCell at " + coordinates + " has no uninitialized directions left.");
            throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
        }
    }


    public void Initialize(MazeRoom room)
    {
        room.Add(this);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = room.settings.floorSprite;
    }

}