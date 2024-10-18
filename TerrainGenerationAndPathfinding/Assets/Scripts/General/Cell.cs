public class Cell : IWalkable
{
    private float altitude;
    private bool walkable;
    private float treshhold;

    public Cell(float altitude, float treshhold)
    {
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
