using UnityEngine;

public class Node
{
    public int x { get; private set; }
    public int y { get; private set; }
    public NodeGrid grid { get; private set; }

    private PlacedObject placedObject;

    public Node(int x, int y, NodeGrid grid)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
    }

    public void SetPlacedObject(PlacedObject placedObj) => this.placedObject = placedObj;
    public PlacedObject GetPlacedObject() => placedObject;
    public void ClearPlacedObject() => placedObject = null;

    public bool canBuild() => placedObject == null;
}
