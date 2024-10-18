using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("Debug only")]
    public bool autoUpdate = false;

    [Space]
    [Header("Generation")]
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    [Range(0,10)]
    public int octaves;
    [Range(0,1f)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;

    public enum DrawMode { NoiseMap, ColourMap, TileMap };
    [Header("Visualize")]
    public DrawMode drawMode = DrawMode.NoiseMap;
    public bool viewCellValue = false;
    public TerrainType[] regions;

    private CustomGrid<Cell> grid;
    public Tilemap tilemap;
    public GameObject plane;

    public Tile walkable;
    public Tile blocked;
    [Range(0,1f)]
    public float treshhold = .4f;

    public CustomGrid<Cell> GetGrid()
    {
        GenerateMap();
        return grid;
    }

    public void GenerateMap()
    {
        grid = new CustomGrid<Cell>(mapWidth, mapHeight, 10f, Vector3.zero,
            (CustomGrid<Cell> g, int x, int y) => new Cell(0, treshhold));

        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,seed, noiseScale, octaves, persistance, lacunarity, offset);
        Color[] colourMap = new Color[mapHeight * mapWidth];
        Tile[,] tiles = new Tile[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //Loop through the cells in the grid and assign corresponding noise value
                Cell cell = grid.GetGridObject(x, y);
                cell.Altitude = noiseMap[x, y];

                //Apply the noise values to the colour map
                if (viewCellValue)
                {
                    colourMap[y * mapWidth + x] = cell.Walkable ? Color.white : Color.black;
                    tiles[x, y] = cell.Walkable ? walkable : blocked;
                }
                else
                {
                    for (int i = 0; i < regions.Length; i++)
                    {
                        if (cell.Altitude <= regions[i].heightValue)
                        {
                            colourMap[y * mapWidth + x] = regions[i].colour;
                            tiles[x, y] = regions[i].tile;
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
            case DrawMode.NoiseMap:
                if (plane != null && !plane.activeSelf) plane.SetActive(true);
                tilemap?.ClearAllTiles();
                display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
                break;
            case DrawMode.ColourMap:
                if (plane != null && !plane.activeSelf) plane.SetActive(true);
                tilemap?.ClearAllTiles();
                display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
                break;
            case DrawMode.TileMap:
                if (plane != null && plane.activeSelf) plane.SetActive(false);
                display.DrawTiles(tilemap, tiles);
                break;
            default:
                break;
        }
    }

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
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float heightValue;
    public Color colour;
    public Tile tile;
}