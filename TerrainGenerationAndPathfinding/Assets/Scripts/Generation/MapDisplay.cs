using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;
    public bool changeSize = false;
    public FilterMode filterMode;
    public TextureWrapMode wrapMode;
    
    public void DrawTexture(Texture2D texture)
    {
        texture.filterMode = filterMode;
        texture.wrapMode = wrapMode;
        textureRenderer.sharedMaterial.mainTexture = texture;
        
        if (changeSize)
            textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawTiles(Tilemap tilemap, Tile[,] tiles)
    {
        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tilemap.SetTile(new Vector3Int(x, y), tiles[x, y]);
            }
        }
    }
}
