public class PathNode<T>
{
    private CustomGrid<PathNode<T>> grid;
    public int x { get; private set; }
    public int y { get; private set; }

    public int gCost;
    public int hCost;
    public int fCost;

    public T nodeObject { get; private set; }


    public PathNode<T> cameFromNode;

    public bool walkable { get; private set; }

    public PathNode(CustomGrid<PathNode<T>> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void SetNodeObject(T nodeObject, bool walkable)
    {
        this.nodeObject = nodeObject;
        this.walkable = walkable;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return $"{x},{y}";
    }
}
