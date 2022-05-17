using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapGenerator : MonoBehaviour
{
    public Texture2D NoiseTexture;
    [Header("")]
    public bool GenerateMap;

    private GameObject cubePrefab;
    private GameObject mapRoot;

    void Start()
    {
        mapRoot = GameObject.Find("MapRoot");
        cubePrefab = Resources.Load<GameObject>("Cube");
    }

    void Update()
    {
        if (GenerateMap)
        {
            GenerateMap = false;
            SpawnMap();

        }
    }

    private void SpawnMap()
    {

        Vector3 pos = new Vector3();

        for (int i = 0; i < 10; i++)
        {
            for (int x = 0; x < NoiseTexture.width; x++)
            {

                for (int y = 0; y < NoiseTexture.height; y++)
                {
                    Color color = NoiseTexture.GetPixel(x, y);
                    Debug.Log(color);
                    pos.z += 1;

                    if (color.r >= 0.3f)
                    {
                        Instantiate<GameObject>(cubePrefab, pos, Quaternion.identity, mapRoot.transform);
                    }
                }

                pos.z = 0;
                pos.x += 1;
            }
        }
        pos.y += 1;
    }
}
