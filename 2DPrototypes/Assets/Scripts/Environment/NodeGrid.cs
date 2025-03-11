using UnityEngine;

public class NodeGrid
{
    public int width { get; private set; }
    public int height { get; private set; }

    public Vector2 cellSize { get; private set; }

    public Vector2 minBounds { get; private set; }
    public Vector2 maxBounds { get; private set; }


    private Node[,] grid;

    private bool debug = false;

    public NodeGrid(int xSize, int ySize, Vector2 cellSize)
    {
        width = xSize;
        height = ySize;
        this.cellSize = cellSize;

        SetBounds();

        grid = GenerateGrid();
    }

    public Node[,] GenerateGrid()
    {
        Node[,] grid = new Node[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Node node = new Node(x, y, this);
                grid[x, y] = node;
            }
        }

        return grid;
    }

    public Node GetNode(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            Node node = grid[x, y];
            return node;
        }
        else
        {
            if (debug) 
                Debug.Log($"Node at index [{x},{y}] not found.");
            return null;
        }
    }

    public Node GetNode(Vector2 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x);
        int y = Mathf.FloorToInt(worldPos.y);

        return GetNode(x, y);
    }

    public void WorldToGridPos(Vector2 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPos.x);
        y = Mathf.FloorToInt(worldPos.y);
    }

    

    public void NodePosition(Node node , out int x, out int y)
    {
        x = 0;
        y = 0;

        if (node == null)
        {
            if (debug)
                Debug.Log($"Node: {node} not found. Default node [0,0]");
            return;
        }

        x = node.x;
        y = node.y;
    }

    private void SetBounds()
    {
        minBounds = new Vector2(0, width);
        maxBounds = new Vector2(0, height);
    }
}
