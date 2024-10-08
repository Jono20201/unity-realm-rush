using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
 {
    [SerializeField][Range(0f, 10f)] float speed = 1f;

    List<Node> path = new();

    private Enemy enemy;

    private GridManager gridManager;
    private PathFinder pathFinder;

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
        enemy = GetComponent<Enemy>();
    }

    void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    private void RecalculatePath(bool resetPath = true)
    {
        Vector2Int coordinates;

        if(resetPath)
        {
            coordinates = pathFinder.StartCoordinates;
        } else {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();

        path.Clear();
        path = pathFinder.GetNewPath(coordinates);

        StartCoroutine(FollowPath());
    }

    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    private IEnumerator FollowPath()
    {
        for(int i = 1; i <path.Count; i++)
        {
            var startPos = transform.position;
            var endPos = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            var travelPercent = 0f;

            transform.LookAt(endPos);

            while(travelPercent < 1)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }

    private void FinishPath() {
        gameObject.SetActive(false);
        enemy.StealGold();
    }
}
