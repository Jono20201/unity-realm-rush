using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int unityGridSize = 10;
    [SerializeField] Vector2Int gridSize = new(10, 10);
    Dictionary<Vector2Int, Node> grid = new();

    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }
    public int UnityGridSize { get { return unityGridSize; } }

    public Node GetNode(Vector2Int coordinates)
    {
        if(grid.TryGetValue(coordinates, out Node node))
            return node;

        return null;
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position) => new()
    {
        x = Mathf.RoundToInt(position.x / unityGridSize),
        y = Mathf.RoundToInt(position.z / unityGridSize)
    };

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates) => new()
    {
        x = coordinates.x * unityGridSize,
        z = coordinates.y * unityGridSize
    };

    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                var coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
        }
    }

    public void ResetNodes()
    {
        foreach(var node in grid.Values)
        {
            node.isExplored = false;
            node.isPath = false;
            node.connectedTo = null;
        }
    }
}
