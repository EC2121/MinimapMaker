using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapGenerator : MonoBehaviour
{
    public Texture2D NoiseTexture;
    [Header("")]
    public bool GenerateMap;

    private GameObject lightDirt;
    private GameObject darkDirt;
    private GameObject grassPrefab;
    private GameObject waterPrefab;
    private GameObject mapRoot;

    void Start()
    {
        mapRoot = GameObject.Find("MapRoot");
        lightDirt = Resources.Load<GameObject>("LightDirt");
        waterPrefab = Resources.Load<GameObject>("Water");
        grassPrefab = Resources.Load<GameObject>("Grass");
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

                InstantiateBlock(pos, (color.r * 10));

            }

            pos.z = 0;
            pos.x += 0.5f;
        }

    }

    private void InstantiateBlock(Vector3 pos, float height)
    {
        GameObject cube = Instantiate<GameObject>(lightDirt, pos, Quaternion.identity, mapRoot.transform);
        cube.transform.localScale = new Vector3(0.5f, height, 0.5f);
        cube.transform.position = new Vector3(pos.x, pos.y + (height * 0.5f), pos.z);
        if (height < 3)
        {
            GameObject water = Instantiate<GameObject>(waterPrefab, new Vector3(pos.x, pos.y + height, pos.z), Quaternion.identity, mapRoot.transform);
        }
        else
        {
            GameObject grass = Instantiate<GameObject>(grassPrefab, new Vector3(pos.x, pos.y + height, pos.z), Quaternion.identity, mapRoot.transform);

        }
    }
}
