public class Cell : IWalkable
{
    private float altitude;
    private bool isWalkable;
    private bool isWater;
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
        get => altitude;
        set { altitude = value; IsWater = altitude < treshhold; }
    }

    public bool IsWater
    {
        get => isWater;
        set { isWater = value; IsWalkable = !value; }
    }

    public bool IsWalkable 
    { 
        get =>  isWalkable; 
        set => isWalkable  = value; 
    }
}
