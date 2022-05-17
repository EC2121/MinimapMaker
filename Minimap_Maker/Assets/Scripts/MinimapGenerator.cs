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


        for (int x = 0; x < NoiseTexture.width; x++)
        {

            for (int y = 0; y < NoiseTexture.height; y++)
            {
                Color color = NoiseTexture.GetPixel(x, y);
                pos.z += 0.5f;

                InstantiateBlock(pos, (int)(color.r * 10));

            }

            pos.z = 0;
            pos.x += 0.5f;
        }


    }


    private void InstantiateBlock(Vector3 pos, int height)
    {
        Debug.Log(height);
        Vector3 newPos = pos;
        for (int i = -1; i < height; i++)
        {
            Instantiate<GameObject>(cubePrefab, newPos, Quaternion.identity, mapRoot.transform);
            newPos.y += 0.5f;
        }
    }
}
