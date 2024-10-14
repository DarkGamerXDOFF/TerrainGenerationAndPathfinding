using System;
using UnityEngine;

public class CustomGrid<TGridObject>
{
    private int width;
    private int height;
    private TGridObject[,] gridArray;
    private Vector3 originPos;
    private float cellSize;
    private bool debug = false;

    public CustomGrid(int width, int height, float cellSize, Vector3 originPos, Func<CustomGrid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPos = originPos;

        gridArray = new TGridObject[width, height];

        for (int y = 0; y < gridArray.GetLength(1); y++)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPos;
    }

    public void GetXY(Vector3 worldPos, out int x, out int y)
    {
        
        x = Mathf.RoundToInt(worldPos.x);
        y = Mathf.RoundToInt(worldPos.y);

        if (debug)
            Debug.Log($"{x}||{y}");
    }

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
        }
    }

    public void SetGridObject(Vector3 worldPos, TGridObject value)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }

        return default(TGridObject);
    }

    public TGridObject GetGridObject(Vector3 worldPos)
    {
        int x, y;
        GetXY(worldPos, out x, out y);

        return GetGridObject(x, y);
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}
