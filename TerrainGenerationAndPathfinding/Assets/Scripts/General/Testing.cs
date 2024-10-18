using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private MapGenerator generator;
    private CustomGrid<Cell> grid;
    private Pathfinding<Cell> pathfinding;

    public bool moveAdjecant = true;

    public Cell startCell;
    public Cell endCell;

    public List<PathNode<Cell>> path;

    private void Start()
    {
        generator = FindObjectOfType<MapGenerator>();
        grid = generator?.GetGrid();
        pathfinding = new Pathfinding<Cell>(grid, moveAdjecant);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startCell = GetNodeAtMousePos();
        }

        if (Input.GetMouseButtonDown(1))
        {
            endCell = GetNodeAtMousePos();
        }

        if (Input.GetKeyDown(KeyCode.Space) && startCell != null && endCell != null)
        {
            GetPath();
        }
        
    }

    private void GetPath() => path = pathfinding.FindPath(startCell, endCell);

    private Cell GetNodeAtMousePos()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition(); ;
        Cell node = grid?.GetGridObject(mouseWorldPos);
        return node;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (path != null && path.Count > 1)
        {
            for (int i = 1; i < path.Count; i++)
            {
                Gizmos.DrawLine(new Vector3(path[i].x, path[i].y), new Vector3(path[i - 1].x, path[i - 1].y));
            }
        }
    }
}
