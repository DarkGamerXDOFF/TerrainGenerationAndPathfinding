using UnityEngine;
using UnityEngine.Rendering;

public class MapGenerator : MonoBehaviour
{
    public bool autoUpdate = false;

    public enum DrawMode {NoiseMap, ColourMap};
    public DrawMode drawMode = DrawMode.NoiseMap;

    [Space]
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

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,seed, noiseScale, octaves, persistance, lacunarity, offset);
        Color[] colourMap = new Color[mapHeight * mapWidth];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].heightValue)
                    {
                        colourMap[y * mapWidth + x] = regions[i].colour;
                        break;
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
                display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
                break;
            case DrawMode.ColourMap:
                display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));

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
}
