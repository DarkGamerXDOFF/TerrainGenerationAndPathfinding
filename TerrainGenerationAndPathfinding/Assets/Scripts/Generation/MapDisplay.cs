using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public enum DrawMode { NOISEMAP, COLOURMAP, RIVEROBS };
    public DrawMode drawMode = DrawMode.NOISEMAP;
    public bool useMeshHeight = true;
    public AnimationCurve meshHeightCurve;
    public float meshHeightMultiplier = 10;


    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public FilterMode filterMode;
    public TextureWrapMode wrapMode;

    public void DrawTerrain(CustomGrid<Cell> grid, float[,] noiseMap, Color[] colourMap)
    {
        int mapWidth = grid.GetWidth();
        int mapHeight = grid.GetHeight();

        switch (drawMode)
        {
            default: break;
            case DrawMode.NOISEMAP:
                if (useMeshHeight)
                    DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve),
                    TextureGenerator.TextureFromHeightMap(noiseMap));
                else
                    DrawMesh(MeshGenerator.GenerateTerrainMesh(grid),
                    TextureGenerator.TextureFromHeightMap(noiseMap));
                break;
            case DrawMode.COLOURMAP:
                if (useMeshHeight)
                    DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve),
                    TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
                else
                   DrawMesh(MeshGenerator.GenerateTerrainMesh(grid),
                    TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
                break;
        }
    }

    private void DrawMesh(MeshData meshData, Texture2D texture)
    {
        texture.filterMode = filterMode;
        texture.wrapMode = wrapMode;
        textureRenderer.sharedMaterial.mainTexture = texture;
        meshFilter.sharedMesh = meshData.CreateMesh();
    }
}
