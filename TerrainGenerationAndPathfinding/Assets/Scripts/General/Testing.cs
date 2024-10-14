using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private MapGenerator generator;
    private CustomGrid<PathNode<Cell>> grid;
    private Pathfinding<Cell> pathfinding;

    public PathNode<Cell> startCell;
    public PathNode<Cell> endCell;

    public List<PathNode<Cell>> path;

    private void Start()
    {
        generator = FindObjectOfType<MapGenerator>();
        grid = generator?.grid;
        pathfinding = new Pathfinding<Cell>(grid);
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
            path = pathfinding.FindPath(startCell.x, startCell.y, endCell.x, endCell.y);
        }
    }

    private PathNode<Cell> GetNodeAtMousePos()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition(); ;
        
        PathNode<Cell> node = grid?.GetGridObject(mouseWorldPos);

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
                //if (path[i + 1] != null)
                //    Gizmos.DrawLine(new Vector3(path[i].x, path[i].y), new Vector3(path[i + 1].x, path[i + 1].y));
                Gizmos.DrawLine(new Vector3(path[i].x, path[i].y), new Vector3(path[i - 1].x, path[i - 1].y));
            }
        }
    }
}
