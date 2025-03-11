using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (GUILayout.Button("Generate Random Location"))
        {
            mapGen.RandomizeOffset();
            mapGen.testing.GenerateMap();
        }

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
                mapGen.testing.GenerateMap();
        }
    }
}
