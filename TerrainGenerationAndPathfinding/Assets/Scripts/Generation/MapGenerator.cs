using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Debug only")]
    public bool autoUpdate = false;
    public Testing testing;

    [Space]
    [Header("Generation")]
    [Header("--Terrain")]
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

    [Header("--Rivers")]
    public float riverDepth = 0.5f;
    float riverStartTreshhold = .7f;

    [Header("Visualize")]
    public bool useFalloffMap = true;
    public Color riverColor;
    public TerrainType[] regions;

    private float[,] falloffMap;

    private List<Cell> riverCells;


    public void GenerateMap(out CustomGrid<Cell> grid, out float[,] noiseMap, out Color[] colourMap)
    {
        grid = new CustomGrid<Cell>(mapWidth, mapHeight,1f, Vector3.zero,
            (CustomGrid<Cell> g, int x, int y) => new Cell(x, y, 0, treshhold));
        
        // Generate noisemap for terrain
        noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,seed, 
            noiseScale, octaves, persistance, lacunarity, offset);

        if (falloffMap == null)
            falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight);

        colourMap = new Color[mapHeight * mapWidth];


        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                Cell cell = grid.GetGridObject(x, y);

                //Apply falloff map
                if (useFalloffMap)
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);

                //Assign corresponding value
                cell.Altitude = noiseMap[x, y];

                //Apply the noise values to the colour map
                for (int i = 0; i < regions.Length; i++)
                {
                    if (cell.Altitude <= regions[i].maxheight)
                    {
                        float temp = Mathf.InverseLerp(i - 1 >= 0 ? regions[i - 1].maxheight : 0,
                            regions[i].maxheight, cell.Altitude);

                        Color colour = regions[i].gradient.Evaluate(temp);
                        colourMap[y * mapWidth + x] = colour;

                        break;
                    }
                }
            }
        }
        RiverGenerator riverGen = new RiverGenerator();
        riverCells = riverGen.GenerateRivers(grid, riverStartTreshhold);

        for (int i = 0; i < riverCells.Count; i++)
        {
            Cell cell = riverCells[i];
            cell.Altitude -= riverDepth;
            colourMap[cell.y * mapWidth + cell.x] = riverColor;
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
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    [Range(0,1f)]
    public float maxheight;
    public Gradient gradient;
}