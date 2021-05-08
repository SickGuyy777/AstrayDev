using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathFinder : MonoBehaviour
{
    private static List<PathFinder> AllPathFinders = new List<PathFinder>();
    
    
    [SerializeField] private bool debugPath = true;

    public Queue<Vector2> currentPath = new Queue<Vector2>();
    public event Action OnUpdatedPath = delegate { };

    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public const int straightCost = 10;
    public const int diagonalCost = 14;

    private Coroutine searchCoroutine;
    private Vector2 currentTarget;
    

    private void Start()
    {
        if (PathMaker.Instance == null)
        {
            Debug.LogError("You must add a PathMaker to the scene before this component can be functional");
            return;
        }
        
        if(grid == null)
            this.grid = PathMaker.Instance.Grid;
        
        AllPathFinders.Add(this);
    }

    private void OnDestroy()
    {
        AllPathFinders.Remove(this);
    }

    public static void UpdateAllPaths(float stepDuration = 0)
    {
        for (int i = 0; i < AllPathFinders.Count; i++)
        {
            PathFinder pathFinder = AllPathFinders[i];
            
            pathFinder.UpdatePath(pathFinder.currentTarget, stepDuration);
        }
    }

    public void UpdatePath(Vector2 target, float stepDuration = 0)
    {
        if(grid == null)
            this.grid = PathMaker.Instance.Grid;
        
        PathNode targetNode = grid.GetObjectFromPosition(target);
        
        if(targetNode == null)
            return;

        currentTarget = target;
        
        if(searchCoroutine != null)
            StopCoroutine(searchCoroutine);

        searchCoroutine = StartCoroutine(FindGridPath(targetNode, SetPath, stepDuration));
    }

    private void SetPath(List<PathNode> path)
    {
        Queue<Vector2> newPath = new Queue<Vector2>();

        for (int i = 0; i < path.Count; i++)
        {
            PathNode pathNode = path[i];
            
            newPath.Enqueue(pathNode.worldPosition);
        }

        currentPath = newPath;
        
        OnUpdatedPath.Invoke();
    }

    private IEnumerator FindGridPath(PathNode endNode, Action<List<PathNode>> OnPathFound = null, float duration = 0)
    {
        if (!endNode.IsWalkable)
            endNode = GetClosestWalkableNode(endNode);
        
        PathNode startNode = grid.GetObjectFromPosition(transform.position);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int y = 0; y < grid.Size.y; y++)
        {
            for (int x = 0; x < grid.Size.x; x++)
            {
                PathNode pathNode = grid.GridObjects[x, y];

                pathNode.gCost = int.MaxValue;
                pathNode.hCost =  CalculateDistance(pathNode, endNode);
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost =  CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();
        
        while (openList.Count > 0)
        {
            if(duration > 0)
                yield return new WaitForSeconds(duration);
            
            PathNode currentNode = GetTheLowestFCostNode(openList);
            
            if (currentNode == endNode)
            {
                List<PathNode> path = CalculatePath(endNode);
                OnPathFound?.Invoke(path);
                yield break;
            }
            
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            for (int i = 0; i < currentNode.Neighbors.Length; i++)
            {
                PathNode neighborNode = currentNode.Neighbors[i];
                
                if(closedList.Contains(neighborNode) || !neighborNode.IsWalkable)
                    continue;

                int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighborNode);
                
                if (tentativeGCost < neighborNode.gCost)
                {
                    neighborNode.cameFromNode = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistance(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode))
                        openList.Add(neighborNode);
                }
            }
        }
    }

    private PathNode GetClosestWalkableNode(PathNode node)
    {
        List<PathNode> walkableNodes = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode> { node };


        while (walkableNodes.Count <= 0)
        {
            List<PathNode> newClosedList = new List<PathNode>();
            
            for (int i = 0; i < closedList.Count; i++)
            {
                PathNode pathNode = closedList[i];

                for (int j = 0; j < pathNode.Neighbors.Length; j++)
                {
                    PathNode neighbor = pathNode.Neighbors[j];
                    
                    if(neighbor.IsWalkable)
                        walkableNodes.Add(neighbor);
                    
                    newClosedList.Add(neighbor);
                }
            }

            closedList = newClosedList;
        }

        return GetPathNodeWithLowestDistance(node, walkableNodes);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode> {endNode};

        PathNode currentNode = endNode;
        
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistance(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.xIndex - b.xIndex);
        int yDistance = Mathf.Abs(a.yIndex - b.yIndex);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return diagonalCost * Mathf.Min(xDistance, yDistance) + straightCost * remaining;
    }

    private PathNode GetPathNodeWithLowestDistance(PathNode current, List<PathNode> nodeList)
    {
        int lowestDistance = int.MaxValue;
        PathNode lowestPathNode = null;

        for (int i = 0; i < nodeList.Count; i++)
        {
            PathNode pathNode = nodeList[i];
            int distance = CalculateDistance(current, pathNode);

            if (distance < lowestDistance)
            {
                lowestPathNode = pathNode;
                lowestDistance = distance;
            }
        }
        
        return lowestPathNode;
    }

    private PathNode GetTheLowestFCostNode(List<PathNode> nodeList)
    {
        PathNode lowestPathNode = nodeList[0];

        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].fCost < lowestPathNode.fCost)
                lowestPathNode = nodeList[i];
        }
        
        return lowestPathNode;
    }
    
    public Queue<Vector2> GetPath() => currentPath;

    private void OnDrawGizmos()
    {
        if(!debugPath  || currentPath == null)
            return;

        Vector2 lastPosition = transform.position;
        
        foreach (Vector2 point in currentPath)
        {
            Gizmos.DrawLine(lastPosition, point);

            lastPosition = point;
        }
    }
}
