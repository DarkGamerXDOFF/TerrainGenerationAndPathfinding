using System.Collections.Generic;
using UnityEngine;

public class RiverGenerator
{
    private CustomGrid<Cell> grid;
    private float treshhold;
    int width = 0;
    int height = 0;
    public List<Cell> GenerateRivers(CustomGrid<Cell> grid, float treshhold)
    {
        this.grid = grid;
        this.treshhold = treshhold;

        width = grid.GetWidth();
        height = grid.GetHeight();
        
        List<Cell> riverCells = new List<Cell>();

        //Get the river starting cell
        Cell startCell = GetStartCell();

        if (startCell == null)
            Debug.Log("No valid altitude found");
        else
            riverCells.Add(startCell);

        Cell currentCell = startCell;

        // Keep moving downhill until we reach a cell below the altitude threshold (e.g., ocean)
        while (currentCell.Altitude > 0.4)
        {
            if (!riverCells.Contains(currentCell))
            {
                riverCells.Add(currentCell);
                currentCell.IsWater = true; // Mark the cell as a river for future use
            }

            Cell nextCell = GetLowestAltCell(currentCell);

            // If there are no lower neighbors, we reached a flat or local minimum
            if (nextCell == null)
            {
                Debug.LogError("Next river cell not found!");
                break;
            }

            // Move to the next cell
            currentCell = nextCell;
        }
        return riverCells;
    }

    private Cell GetStartCell()
    {
        Cell startCell = grid.GetGridObject(0,0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell currentCell = grid.GetGridObject(x, y);

                if (currentCell.Altitude < treshhold) continue;

                if (currentCell.Altitude > startCell.Altitude)
                {
                    startCell = currentCell;
                }
            }
        }

        return startCell;
    }

    private Cell GetLowestAltCell(Cell current)
    {
        List<Cell> neighbours = GetNeighbourCells(current);

        Cell lowestCell = null;

        foreach (Cell cell in neighbours)
        {
            if (lowestCell == null || (cell.Altitude <= lowestCell.Altitude && !cell.IsWater))
            {
                lowestCell = cell;
            }
        }

        return lowestCell;
    }

    private List<Cell> GetNeighbourCells(Cell current)
    {
        List<Cell> neighbours = new List<Cell>();

        int x = current.x;
        int y = current.y;

        //Up
        if (y + 1 <= height)
            neighbours.Add(grid.GetGridObject(x, y + 1));
        //Down
        if (y - 1 >= 0)
            neighbours.Add(grid.GetGridObject(x, y - 1));
        //Right
        if (x + 1 <= width)
            neighbours.Add(grid.GetGridObject(x+1, y));
        //Down
        if (x - 1 >= 0)
            neighbours.Add(grid.GetGridObject(x - 1, y));

        return neighbours;
    }
}
