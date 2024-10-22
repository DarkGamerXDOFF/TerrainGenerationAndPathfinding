using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public FilterMode filterMode;
    public TextureWrapMode wrapMode;

    public void DrawTexture(Texture2D texture)
    {
        texture.filterMode = filterMode;
        texture.wrapMode = wrapMode;
        textureRenderer.sharedMaterial.mainTexture = texture;
    }
    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        DrawTexture(texture);
        meshFilter.sharedMesh = meshData.CreateMesh();
    }
}
