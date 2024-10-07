using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;

    public FilterMode filterMode;
    public TextureWrapMode wrapMode;
    
    public void DrawTexture(Texture2D texture)
    {
        texture.filterMode = filterMode;
        texture.wrapMode = wrapMode;
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }
}
