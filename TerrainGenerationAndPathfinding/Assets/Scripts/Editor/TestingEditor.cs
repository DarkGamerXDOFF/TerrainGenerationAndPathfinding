using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Testing))]
public class TestingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Testing testing = (Testing)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            testing.GenerateMap();
        }
        
    }
}
