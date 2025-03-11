using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager I;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Vector2 cellSize;
    
    public NodeGrid grid { get; private set; }

    [SerializeField] private Tilemap groundTM;
    [SerializeField] private Tile groundTile;
    
    private void Awake()
    {
        if (I == null)
            I = this;
        else
            Destroy(this);

        grid = new NodeGrid(width, height, cellSize);
    }

    private void Start()
    {
        GenerateTM();
    }

    private void GenerateTM()
    {
        if (groundTile == null)
        {
            Debug.LogError("Ground tile not set!");
            return;
        }
        if (groundTM == null)
        {
            Debug.LogError("Ground tilemap not set!");
            return;
        }

        for (int y = 0; y < grid.height; y++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                int xPos = 0;
                int yPos = 0;
                
                Node node = grid.GetNode(x, y);
                grid.NodePosition(node, out xPos, out yPos);
                Vector3Int pos = new Vector3Int(xPos, yPos);

                groundTM.SetTile(pos, groundTile);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (grid == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(grid.minBounds, 0.2f);
        Gizmos.DrawSphere(grid.maxBounds, 0.2f);
    }
}
