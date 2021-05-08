
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public PathNode cameFromNode;
    public int gCost;
    public int hCost = 0;
    public int fCost;

    public bool IsWalkable;
    public bool IsObstacleByBake;

    public readonly bool hasObstacle = false;
    public readonly Grid<PathNode> grid;
    public readonly int xIndex;
    public readonly int yIndex;
    public readonly Vector2 worldPosition;
    public readonly LayerMask obstacleMask;
    public readonly int obstaclePadding;
    public PathNode[] Neighbors { get; private set; }


    public PathNode(Grid<PathNode> grid, int xIndex, int yIndex, LayerMask obstacleMask, int obstaclePadding)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.grid = grid;
        
        this.worldPosition = grid.GetWorldPositionFromIndex(xIndex, yIndex);
        
        this.obstacleMask = obstacleMask;
        this.obstaclePadding = obstaclePadding;
        
        hasObstacle = HasObstacle();
        IsObstacleByBake = hasObstacle;
        IsWalkable = !hasObstacle;

        grid.OnGridUpdated += UpdateNeighbors;
    }

    public void CalculateFCost() => fCost = gCost + hCost;

    private void UpdateNeighbors()
    {
        grid.OnGridUpdated -= UpdateNeighbors;

        this.Neighbors = GetNeighbors();
        
        if(hasObstacle)
            ApplyPadding(obstaclePadding);
    }

    private void ApplyPadding(int paddingAmount)
    {
        if(paddingAmount <= 0)
            return;
        
        foreach (PathNode pathNode in GetPaddingNeighbors())
        {
            if (pathNode.IsWalkable)
            {
                pathNode.IsWalkable = false;
                pathNode.IsObstacleByBake = true;
            }
            
            if(paddingAmount - 1 > 0)
                pathNode.ApplyPadding(paddingAmount - 1);
        }
    }
    
    private bool HasObstacle()
    {
        Collider2D col = Physics2D.OverlapBox(worldPosition, new Vector2(grid.CellSize / 1.6f, grid.CellSize / 1.6f), 0);
            
        return !((col == null || col.isTrigger) || col != null && IsLayerInObstacleMask(col.gameObject.layer));
    }
    
    private bool IsLayerInObstacleMask(int layer) => obstacleMask.value != (obstacleMask | (1 << layer));
    
    private PathNode[] GetPaddingNeighbors()
    {
        List<PathNode> newNeighbors = new List<PathNode>();
        Vector2Int gridPos = new Vector2Int(xIndex, yIndex);

        PathNode topNeighbor = grid.GetRelativeObject(gridPos, 0, 1);
        PathNode bottomNeighbor = grid.GetRelativeObject(gridPos, 0, -1);
        PathNode rightNeighbor = grid.GetRelativeObject(gridPos, 1, 0);
        PathNode leftNeighbor = grid.GetRelativeObject(gridPos, -1, 0);
        
        if(topNeighbor != null)
            newNeighbors.Add(topNeighbor);
        if(bottomNeighbor != null)
            newNeighbors.Add(bottomNeighbor);
        if(rightNeighbor != null)
            newNeighbors.Add(rightNeighbor);
        if(leftNeighbor != null)
            newNeighbors.Add(leftNeighbor);
        
        return newNeighbors.ToArray();
    }
    
    private PathNode[] GetNeighbors()
    {
        List<PathNode> newNeighbors = new List<PathNode>();
        Vector2Int gridPos = new Vector2Int(xIndex, yIndex);

        PathNode topNeighbor = grid.GetRelativeObject(gridPos, 0, 1);
        PathNode bottomNeighbor = grid.GetRelativeObject(gridPos, 0, -1);
        PathNode rightNeighbor = grid.GetRelativeObject(gridPos, 1, 0);
        PathNode leftNeighbor = grid.GetRelativeObject(gridPos, -1, 0);
        
        PathNode topRightNeighbor = grid.GetRelativeObject(gridPos, 1, 1);
        PathNode bottomRightNeighbor = grid.GetRelativeObject(gridPos, 1, -1);
        PathNode bottomLeftNeighbor = grid.GetRelativeObject(gridPos, -1, -1);
        PathNode topLeftNeighbor = grid.GetRelativeObject(gridPos, -1, 1);
        
        if(topNeighbor != null)
            newNeighbors.Add(topNeighbor);
        if(bottomNeighbor != null)
            newNeighbors.Add(bottomNeighbor);
        if(rightNeighbor != null)
            newNeighbors.Add(rightNeighbor);
        if(leftNeighbor != null)
            newNeighbors.Add(leftNeighbor);
        if(topRightNeighbor != null)
            newNeighbors.Add(topRightNeighbor);
        if(bottomRightNeighbor != null)
            newNeighbors.Add(bottomRightNeighbor);
        if(bottomLeftNeighbor != null)
            newNeighbors.Add(bottomLeftNeighbor);
        if(topLeftNeighbor != null)
            newNeighbors.Add(topLeftNeighbor);
        
        return newNeighbors.ToArray();
    }
}
