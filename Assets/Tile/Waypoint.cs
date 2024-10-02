using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] bool isPlacable;
    [SerializeField] Tower towerPrefab;

    public bool IsPlacable { get { return isPlacable; } }

    private void OnMouseDown() {
        if(!isPlacable) return;

        var isPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);

        isPlacable = false;
    }
}
