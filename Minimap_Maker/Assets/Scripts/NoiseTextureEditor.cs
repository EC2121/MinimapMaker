using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoiseTextureGenerator))]
public class NoiseTextureEditor : Editor
{
    // Start is called before the first frame update

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        NoiseTextureGenerator generator = (NoiseTextureGenerator)target;
        if (GUILayout.Button("Generate Texture"))
        {
            generator.Generate();
        }
    }



}
