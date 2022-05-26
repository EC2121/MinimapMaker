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

        if (generator.RandomBiome)
            GUI.enabled = false;
        generator.BiomeSelector = (Biome)EditorGUILayout.EnumPopup("Biome:", generator.BiomeSelector);

        if (generator.RandomFade)
            GUI.enabled = false;
        else
            GUI.enabled = true;

        generator.FadeType = (FadeType)EditorGUILayout.EnumPopup("Fade:", generator.FadeType);

        GUI.enabled = true;
        if (GUILayout.Button("Generate New Map"))
        {
            generator.GenerateTexture();
            generator.RepositionMap();
        }



    }
}
