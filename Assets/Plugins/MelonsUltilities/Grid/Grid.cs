using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid<T>
{
    public Vector2Int Size { get; private set; }
    public float CellSize { get; private set; } 
    public T[,] GridObjects { get; private set; }
    public Vector2 OriginPosition { get; private set; }
    private Vector2 offset;
    public bool Updated { get; private set; } = true;

    public event Action OnGridUpdated = delegate { };


    public Grid(Vector2Int gridSize, float cellSize, Func<Grid<T>, int, int, T> createdGridObj, Vector2 originPosition = default)
    {
        Update(gridSize, cellSize, createdGridObj, originPosition);
    }

    public void Update(Vector2Int gridSize, float cellSize, Func<Grid<T>, int, int, T> createdGridObj, Vector2 originPosition = default)
    {
        if (Updated == false)
        {
            Debug.LogError("Cant update while updating");
            return;
        }
        
        Updated = false;
        
        this.Size = new Vector2Int(gridSize.x, gridSize.y);
        this.CellSize = cellSize;
        this.OriginPosition = originPosition;
        offset = originPosition + (-(Size * new Vector2(CellSize, CellSize)) / 2f);
        
        GridObjects = new T[Size.x, Size.y];
        
        for (int y = 0; y < Size.y; y++)
        {
            for (int x = 0; x < Size.x; x++)
            {
                GridObjects[x, y] = createdGridObj(this, x, y);
            }
        }
        
        Updated = true;
        OnGridUpdated.Invoke();
    }

    public List<T> GetGridObjects() => GridObjects.Cast<T>().ToList();

    private bool WithinBounds(int xIndex, int yIndex) => xIndex >= 0 && yIndex >= 0 && xIndex < Size.x  && yIndex < Size.y;

    public T GetObjectFromPosition(Vector2 worldPosition)
    {
        Vector2 pos = (worldPosition - OriginPosition) - offset;
        
        int xIndex = Mathf.FloorToInt((pos.x / CellSize));
        int yIndex = Mathf.FloorToInt((pos.y / CellSize));

        if (!WithinBounds(xIndex, yIndex))
            return default;

        return GetObjectFromIndex(xIndex, yIndex);
    }

    public T GetObjectFromIndex(int xIndex, int yIndex) => GridObjects[xIndex, yIndex];
    
    public Vector2 GetWorldPositionFromIndex(int xIndex, int yIndex)
    {
        if (!WithinBounds(xIndex, yIndex))
        {
            Debug.LogWarning("Grid Position Out Of Bounds: " + new Vector2Int(xIndex, yIndex));
            return GetClosestCellPositionFromIndex(xIndex, yIndex);
        }
        
        return (new Vector2(xIndex, yIndex) * CellSize) + offset + new Vector2(CellSize / 2f, CellSize / 2f); 
    }

    private Vector2 GetClosestCellPositionFromIndex(int xIndex, int yIndex)
    {
        xIndex = Mathf.Clamp(xIndex, 0, Size.x - 1);
        yIndex = Mathf.Clamp(yIndex, 0, Size.y - 1);
        
        return (new Vector2(xIndex, yIndex) * CellSize) + offset + new Vector2(CellSize / 2f, CellSize / 2f); 
    }

    public Vector2 GetCellPosition(Vector2 worldPosition)
    {
        Vector2 pos = (worldPosition - OriginPosition) - offset;
        
        int xIndex = Mathf.FloorToInt((pos.x / CellSize));
        int yIndex = Mathf.FloorToInt((pos.y / CellSize));
        
        return GetWorldPositionFromIndex(xIndex, yIndex);
    }

    public T GetRelativeObject(Vector2Int gridPos, int xTranslation, int yTranslation)
    {
        Vector2Int newGridPos = new Vector2Int(gridPos.x + xTranslation, gridPos.y + yTranslation);
        
        if (!WithinBounds(newGridPos.x, newGridPos.y))
            return default;

        return GetObjectFromIndex(newGridPos.x, newGridPos.y);
    }
    
    public void DrawGrid()
    {
        if(!Updated)
            return;
        
        for (int y = 0; y < Size.y; y++)
        {
            for (int x = 0; x < Size.x; x++)
            {
                Gizmos.DrawWireCube(GetWorldPositionFromIndex(x, y), new Vector3(CellSize, CellSize));
            }
        }
    }
}
