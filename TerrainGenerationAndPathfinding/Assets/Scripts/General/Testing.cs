using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;
using System.Collections.Generic;

public class Testing : MonoBehaviour
{
    private Pathfinding pathfinding;
    private CustomGrid<PathNode> grid;

    public PathNode startNode;
    public PathNode endNode;

    public Vector2Int gridSize;
    private void Start()
    {
        //pathfinding = new Pathfinding(gridSize.x, gridSize.y);
        //grid = pathfinding.GetGrid();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = UtilsClass.GetMouseWorldPosition();
            //grid.GetXY(mouseWorldPos, out int x, out int y);
            startNode = grid.GetGridObject(mouseWorldPos);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (startNode == null)
                return;

            Vector3 mouseWorldPos = UtilsClass.GetMouseWorldPosition();
            //grid.GetXY(mouseWorldPos, out int x, out int y);
            endNode = grid.GetGridObject(mouseWorldPos);

            if (endNode == null)
                return;

            List<PathNode> path = pathfinding.FindPath(startNode.x, startNode.y, endNode.x, endNode.y);

            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Vector3 start = new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f;
                    Vector3 end = new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f;
                    Debug.DrawLine(start, end, Color.green, 10f);
                }
            }
        }   
    }
}
