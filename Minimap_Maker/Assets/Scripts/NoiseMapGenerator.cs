using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
public class NoiseMapGenerator : MonoBehaviour
{
    [Header("Desired Size")]
    public int Width = 32;
    public int Height = 32;
    [Header("")]
    public bool GenerateAndSave;
    void Start()
    {
        
    }

    void Update()
    {
        if (GenerateAndSave)
        {
            GenerateAndSave = false;
            GenerateAndSaveTexture();
        }
    }

    private void GenerateAndSaveTexture()
    {

        Texture2D texture = new Texture2D(Width, Height);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                float xCoord = (float)x / Width * 20;
                float yCoord = (float)y / Height * 20;

                float noiseFloat = Mathf.PerlinNoise(xCoord, yCoord);
                Color GreyScaleColor = new Color(noiseFloat, noiseFloat, noiseFloat);
                texture.SetPixel(x, y, GreyScaleColor);

            }
        }
        
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Assets" + "/../" + "Noise.png", bytes);
    }
}
