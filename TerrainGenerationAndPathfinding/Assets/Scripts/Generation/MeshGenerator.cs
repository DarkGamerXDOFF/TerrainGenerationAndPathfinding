using UnityEngine;

public static class MeshGenerator 
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        MeshData meshData = new MeshData(width, height);
        int vertIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                meshData.verts[vertIndex] = new Vector3(x -.5f, heightCurve.Evaluate(heightMap[x,y]) * heightMultiplier, y-.5f);
                meshData.uvs[vertIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width-1 && y < height - 1)
                {
                    meshData.AddTriangle(vertIndex, vertIndex + width, vertIndex + 1);
                    meshData.AddTriangle(vertIndex + 1, vertIndex + width, vertIndex + width + 1);
                }

                vertIndex++;
            }
        }

        return meshData;
    }
    public static MeshData GenerateTerrainMesh(CustomGrid<Cell> grid)
    {
        int width = grid.GetWidth();
        int height = grid.GetHeight();

        MeshData meshData = new MeshData(width, height);
        int vertIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                meshData.verts[vertIndex] = new Vector3(x - .5f, 0, y - .5f);
                meshData.uvs[vertIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertIndex, vertIndex + width, vertIndex + 1);
                    meshData.AddTriangle(vertIndex + 1, vertIndex + width, vertIndex + width + 1);
                }

                vertIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData 
{
    public Vector3[] verts;
    public int[] tris;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        verts = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        tris = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }
    public void AddTriangle(int a, int b, int c)
    {
        tris[triangleIndex] = a;
        tris[triangleIndex + 1] = b;
        tris[triangleIndex + 2] = c;
        triangleIndex += 3;
    }
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }
}
