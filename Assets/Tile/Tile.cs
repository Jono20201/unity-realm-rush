using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlacable;
    [SerializeField] Tower towerPrefab;

    GridManager gridManager;
    PathFinder pathFinder;
    Vector2Int coordinates = new();

    public bool IsPlacable { get { return isPlacable; } }

    private void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    private void Start() {
        if(gridManager != null) {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if(!isPlacable) {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    private void OnMouseDown() {
        if(gridManager.GetNode(coordinates).isWalkable && !pathFinder.WillBlockPath(coordinates)) {
            if(isPlacable) {
                var wasPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);

                if(wasPlaced) {
                    gridManager.BlockNode(coordinates);
                    pathFinder.NotifyRecievers();
                }
            }
        }
    }
}
