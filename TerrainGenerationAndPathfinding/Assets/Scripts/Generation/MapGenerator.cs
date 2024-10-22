using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    [Header("Debug only")]
    public bool autoUpdate = false;

    [Space]
    [Header("Generation")]
    public int mapWidth;
    public int mapHeight;
    public float cellSize = 1f;
    public float noiseScale;
    [Range(0,10)]
    public int octaves;
    [Range(0,1f)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    [Range(0, 1f)]
    public float treshhold = .4f;

    public enum DrawMode {NOISEMAP, COLOURMAP};

    [Header("Visualize")]
    public DrawMode drawMode = DrawMode.NOISEMAP;
    public bool useFalloffMap = true;
    public bool viewCellValue = false;
    public bool useMeshHeight = true;
    public float meshHeightMultiplier = 10;
    public AnimationCurve meshHeightCurve;
    public TerrainType[] regions;

    private CustomGrid<Cell> grid;
    private float[,] falloffMap;

    private void Awake()
    {
        if (Instance != null)
            Instance = this;
        else
            Destroy(this);

        falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight);
    }
    public CustomGrid<Cell> GetGrid()
    {
        GenerateMap();
        return grid;
    }

    public void GenerateMap()
    {
        grid = new CustomGrid<Cell>(mapWidth, mapHeight,1f, Vector3.zero,
            (CustomGrid<Cell> g, int x, int y) => new Cell(x, y, 0, treshhold));

        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,seed, 
            noiseScale, octaves, persistance, lacunarity, offset);
        Color[] colourMap = new Color[mapHeight * mapWidth];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //Apply falloff map
                if (useFalloffMap)
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);

                //Loop through the cells in the grid and assign corresponding noise value
                Cell cell = grid.GetGridObject(x, y);
                cell.Altitude = noiseMap[x, y];


                //Apply the noise values to the colour map
                if (viewCellValue)
                {
                    colourMap[y * mapWidth + x] = cell.Walkable ? Color.white : Color.black;
                }
                else
                {
                    for (int i = 0; i < regions.Length; i++)
                    {
                        if (cell.Altitude <= regions[i].maxheight)
                        {
                            float temp = Mathf.InverseLerp(i-1 >= 0 ? regions[i-1].maxheight: 0, 
                                regions[i].maxheight, cell.Altitude);
                            Color colour = regions[i].gradient.Evaluate(temp);
                            colourMap[y * mapWidth + x] = colour;
                            break;
                        }
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        
        if (display == null)
        {
            Debug.LogError("MapDisplay has not been assigned properly!");
            return;
        }

        switch (drawMode)
        {
            default: break;
            case DrawMode.NOISEMAP: 
                if (useMeshHeight)
                    display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve),
                    TextureGenerator.TextureFromHeightMap(noiseMap));
                else
                    display.DrawMesh(MeshGenerator.GenerateTerrainMesh(grid),
                    TextureGenerator.TextureFromHeightMap(noiseMap)); 
                break;
            case DrawMode.COLOURMAP: 
                if (useMeshHeight)
                    display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve),
                    TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
                else
                    display.DrawMesh(MeshGenerator.GenerateTerrainMesh(grid),
                    TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight)); 
                break;
        }
    }

    public void RandomizeOffset() => offset = new Vector2(Random.Range(-100000, 100000), 
        Random.Range(-100000, 100000));

    private void OnValidate()
    {
        if (mapWidth < 1)
            mapWidth = 1;

        if (mapHeight < 1)
            mapHeight = 1;

        if (lacunarity < 1)
            lacunarity = 1;

        if (octaves < 0)
            octaves = 0;

        if (useFalloffMap == true)
            falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight);
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    [Range(0,1f)]
    public float maxheight;
    public Gradient gradient;
}