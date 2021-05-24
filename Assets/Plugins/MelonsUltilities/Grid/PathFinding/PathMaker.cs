using UnityEngine;

[ExecuteInEditMode]
public class PathMaker : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new Vector2Int(40, 20);
    [SerializeField] private float cellSize = .5f;
    
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private int obstaclePadding = 1;
    [SerializeField] private bool showGrid = true;
    
    [SerializeField] private bool autoBake = false;

    public static PathMaker Instance;
    public Grid<PathNode> Grid { get; private set; }
    public bool AutoBake => autoBake;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Cannot have more than one Path Maker In Scene");
            DestroyImmediate(this.gameObject);
        }

        Grid ??= GetNewGrid();
        Bake();
    }

    private void OnValidate()
    {
        Grid ??= GetNewGrid();
        
        if(gridSize.x <= 0)
            gridSize = new Vector2Int(1, gridSize.y);
        if(gridSize.y <= 0)
            gridSize = new Vector2Int(gridSize.x, 1);
        if (cellSize <= 0)
            cellSize = .1f;
        if (obstaclePadding <= 0 || obstaclePadding > 10)
            obstaclePadding = Mathf.Clamp(obstaclePadding, 0, 10);
        
        if(autoBake)    
            Bake();    
    }

    public void Bake()
    {
        Grid ??= GetNewGrid();
        
        if(!Grid.Updated)
            return;
        
        Grid.Update(gridSize, cellSize,(grid, x, y) => new PathNode(grid, x, y, obstacleMask, obstaclePadding), Vector2.zero);

        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    }

    private Grid<PathNode> GetNewGrid() => new Grid<PathNode>(gridSize, cellSize, (grid, x, y) => new PathNode(grid, x, y, obstacleMask, obstaclePadding));

    public void SetAutoBake(bool auto)
    {
        this.autoBake = auto;
        
        if(auto)
            Bake();
    }

    private void OnDrawGizmos()
    {
        if(!showGrid)
            return;
        
        Grid.DrawGrid();

        Gizmos.color = new Color(255, 0, 0, 100f);
        
        if(Grid?.GridObjects == null)
            return;
        
        foreach (PathNode pathNode in Grid.GridObjects)
        {
            if (pathNode == null)
                continue;

            if (!pathNode.IsWalkable)
            {
                Gizmos.DrawLine(pathNode.worldPosition - new Vector2(cellSize / 2, cellSize / 2), pathNode.worldPosition + new Vector2(cellSize / 2, cellSize / 2));
                Gizmos.DrawLine(pathNode.worldPosition - new Vector2(cellSize / 2, -(cellSize / 2)), pathNode.worldPosition + new Vector2(cellSize / 2, -(cellSize / 2)));
            }
        }
    }

    private void OnDestroy()
    {
        if(Instance != this)
            return;
        
        bool hasAPathFinder = FindObjectOfType<PathFinder>();
        
        if(hasAPathFinder)
            Debug.LogError("You currently have PathFinders in the scene, they will not be functional without a PathMaker in the scene!");
    }
}
