public class Cell
{
    private float altitude;
    public bool walkable;
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
            walkable = altitude >= treshhold;
        }
    }
}
