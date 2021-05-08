
using UnityEngine;

public class PathObstacle : MonoBehaviour
{
    private PathNode currentNode;
    private Grid<PathNode> grid;
    
    
    private void Start()
    {
        if (PathMaker.Instance == null)
        {
            Debug.LogError("You must add a PathMaker to the scene before this component can be functional");
            return;
        }
        
        if(grid == null)
            this.grid = PathMaker.Instance.Grid;
    }

    private void Update() => UpdateCurrentNode();

    private void UpdateCurrentNode()
    {
        PathNode newNode = grid.GetObjectFromPosition(transform.position);

        if (currentNode != newNode)
        {
            if (currentNode != null && !currentNode.IsObstacleByBake)
                currentNode.IsWalkable = true;
            
            newNode.IsWalkable = false;
            PathFinder.UpdateAllPaths();

            currentNode = newNode;
        }
    }
}
