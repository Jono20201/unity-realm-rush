using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    [SerializeField] Vector2Int destinationCoordinates;

    public Vector2Int StartCoordinates { get { return startCoordinates; } }
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }
    
    Node startNode;
    Node destinationNode;
    Node currentSearchNode;
    Dictionary<Vector2Int, Node> reached = new();
    Queue<Node> frontier = new();

    GridManager gridManager;

    Dictionary<Vector2Int, Node> grid = new();

    Vector2Int[] directions = {
        Vector2Int.right,
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.down
    };

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();

        if(gridManager != null) {
            grid = gridManager.Grid;

            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
    }

    private void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath() {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates) {
        gridManager.ResetNodes();
        BreathFirstSearch(coordinates);

        return BuildPath();
    }

    private void ExploreNeighbours()
    {
        var neighbours = new List<Node>();

        foreach(var direction in directions)
        {
            var neighbourCoordinates = currentSearchNode.coordinates + direction;

            if(grid.ContainsKey(neighbourCoordinates))
            {
                neighbours.Add(grid[neighbourCoordinates]);
            }
        }

        foreach(var neighbour in neighbours)
        {
            if(!reached.ContainsKey(neighbour.coordinates) && neighbour.isWalkable)
            {
                neighbour.connectedTo = currentSearchNode;
                frontier.Enqueue(neighbour);
                reached.Add(neighbour.coordinates, neighbour);
            }
        }
    }

    private void BreathFirstSearch(Vector2Int coordinates) {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;
        
        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while(frontier.Count > 0 && isRunning) {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbours();

            if(currentSearchNode.coordinates == destinationCoordinates) {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath() {
        var path = new List<Node>();
        var currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while(currentNode.connectedTo != null) {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates) {
        if(grid.ContainsKey(coordinates)) {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;
            var newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if(newPath.Count <= 1) {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyRecievers() {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
