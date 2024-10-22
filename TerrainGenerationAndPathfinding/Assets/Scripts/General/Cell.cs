public class Cell : IWalkable
{
    private float altitude;
    private bool walkable;
    private float treshhold;
    public int x;
    public int y;

    public Cell(int x, int y ,float altitude, float treshhold)
    {
        this.x = x;
        this.y = y;
        Altitude = altitude;
        this.treshhold = treshhold;
    }

    public float Altitude
    {
        get
        {
            return altitude;
        }
        set
        {
            altitude = value;
            Walkable = altitude >= treshhold;
        }
    }

    public bool Walkable 
    { 
        get =>  walkable; 
        set => walkable  = value; 
    }
}
