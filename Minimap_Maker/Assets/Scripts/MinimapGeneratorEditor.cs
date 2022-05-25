using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MinimapGenerator))]
public class MinimapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MinimapGenerator generator = (MinimapGenerator)target;
        if (GUILayout.Button("Generate New Map"))
        {
            generator.GenerateTexture();
            generator.RepositionMap();
        }
    }
}
