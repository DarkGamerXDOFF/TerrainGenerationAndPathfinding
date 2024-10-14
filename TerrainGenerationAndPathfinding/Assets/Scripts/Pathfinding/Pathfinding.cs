using System.Collections.Generic;
using UnityEngine;

public class Pathfinding<T>
{
    private const int MOVE_STRAIGT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private CustomGrid<PathNode<T>> grid;

    private List<PathNode<T>> openList;
    private List<PathNode<T>> closedList;

    public Pathfinding(CustomGrid<PathNode<T>> grid)
    {
        this.grid = grid;
    }

    public List<PathNode<T>> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode<T> startNode = grid.GetGridObject(startX, startY);
        PathNode<T> endNode = grid.GetGridObject(endX, endY);
        
        openList = new List<PathNode<T>> { startNode };
        closedList = new List<PathNode<T>>();

        for (int y = 0; y < grid.GetHeight(); y++)
        {
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                PathNode<T> pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode<T> currentNode = GetLowestFCost(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<PathNode<T>> neighbours = GetNeighbourList(currentNode);

            foreach (PathNode<T> neighbourNode in neighbours)
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.walkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentitiveGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);

                if (tentitiveGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentitiveGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        //Out of nodes in open list
        return null;
    }

    private List<PathNode<T>> GetNeighbourList(PathNode<T> currentNode)
    {
        List<PathNode<T>> neighbourList = new List<PathNode<T>>();

        if (currentNode.x - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));

            //Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));

            //Left Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            //Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));

            //Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));

            //Right Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }

        //Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));

        //Up
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    private PathNode<T> GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode<T>> CalculatePath(PathNode<T> endNode)
    {
        List<PathNode<T>> path = new List<PathNode<T>> { endNode };

        PathNode<T> currentNode = endNode;

        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();

        return path;
    }

    private int CalculateDistanceCost(PathNode<T> a, PathNode<T> b)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDist - yDist);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDist, yDist) + MOVE_STRAIGT_COST * remaining;
    }

    private PathNode<T> GetLowestFCost(List<PathNode<T>> pathNodeList)
    {
        PathNode<T> lowestFCostNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
                lowestFCostNode = pathNodeList[i];
        }

        return lowestFCostNode;
    }

    private void DebugNodeList(List<PathNode<T>> nodeList)
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            Debug.Log(nodeList[i]);
        }
    }
}