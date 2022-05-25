using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapGenerator : MonoBehaviour
{

    public int Width;
    public int Height;

    public float MaxHeight = 15;
    public int GrassHeight = 7;
    public int SandHeight = 6;
    public int WaterHeight = 3;
    public int SnowHeight = 13;
    [Header("")]
    public bool GenerateMap;

    private GameObject lightDirt;
    private GameObject darkDirt;
    private GameObject grassPrefab;
    private GameObject waterPrefab;
    private GameObject mapRoot;
    private Vector3 planeOffset = new Vector3(0, 0.01f, 0);
    private Texture2D noiseTexture;

    private GameObject[,] mapObjects;


    void Start()
    {
        mapRoot = GameObject.Find("MapRoot");
        lightDirt = Resources.Load<GameObject>("StandardCube");
        waterPrefab = Resources.Load<GameObject>("WaterDirt");
        grassPrefab = Resources.Load<GameObject>("Grass");
    }

    void Update()
    {
        if (GenerateMap)
        {
            GenerateMap = false;
            GenerateTexture();
            if (mapObjects[0, 0] != null)
            {
                RepositionMap();
            }
            else
            {
                SpawnMap();
            }

        }
    }


    private void GenerateTexture()
    {

        if (mapObjects == null)
        {
            mapObjects = new GameObject[Width, Height];
        }

        if (noiseTexture != null && (noiseTexture.width != Width || noiseTexture.height != Height))
        {
            Array.Clear(mapObjects, 0, mapObjects.Length);
            mapObjects = new GameObject[Width, Height];
        }



        noiseTexture = new Texture2D(Width, Height);
        System.Random rand = new System.Random(System.DateTime.Now.Millisecond);

        int randOffSet = rand.Next(1, 100);
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                float xCoord = (float)x / Width * 2;
                float yCoord = (float)y / Height * 2;

                float noiseFloat = Mathf.PerlinNoise(xCoord + randOffSet, yCoord + randOffSet);
                Color GreyScaleColor = new Color(noiseFloat, noiseFloat, noiseFloat);
                noiseTexture.SetPixel(x, y, GreyScaleColor);

            }
        }

        noiseTexture.filterMode = FilterMode.Point;
        noiseTexture.wrapMode = TextureWrapMode.Clamp;
        noiseTexture.Apply();
    }
    private void SpawnMap()
    {

        Vector3 pos = new Vector3();


        for (int x = 0; x < Width; x++)
        {

            for (int y = 0; y < Height; y++)
            {

                Color color = noiseTexture.GetPixel(x, y);
                pos.z += 0.5f;

                mapObjects[x, y] = InstantiateBlock(pos, (color.r * MaxHeight));

            }

            pos.z = 0;
            pos.x += 0.5f;
        }

    }

    private void RepositionMap()
    {
        Vector3 pos = new Vector3();
        for (int x = 0; x < Width; x++)
        {

            for (int y = 0; y < Height; y++)
            {

                Color color = noiseTexture.GetPixel(x, y);
                pos.z += 0.5f;

                for (int i = 1; i < mapObjects[x, y].transform.childCount; i++)
                {
                    if (mapObjects[x, y].transform.GetChild(i).gameObject.activeInHierarchy)
                    {
                        mapObjects[x, y].transform.GetChild(i).transform.Translate(-planeOffset);
                    }
                }

                mapObjects[x, y].transform.localScale = new Vector3(1, color.r * MaxHeight + 0.1f, 1);

                mapObjects[x, y].transform.GetChild(1).gameObject.SetActive(color.r * MaxHeight >= GrassHeight && color.r * MaxHeight < SnowHeight); // grass
                mapObjects[x, y].transform.GetChild(2).gameObject.SetActive(color.r * MaxHeight < WaterHeight); // water
                mapObjects[x, y].transform.GetChild(3).gameObject.SetActive(color.r * MaxHeight >= WaterHeight && color.r * MaxHeight < GrassHeight); //sand
                mapObjects[x, y].transform.GetChild(4).gameObject.SetActive(color.r * MaxHeight >= SnowHeight); //snow

                mapObjects[x, y].transform.position = new Vector3(pos.x, pos.y /*+ ((color.r * MaxHeight) * 0.5f)*/, pos.z);
                for (int i = 1; i < mapObjects[x, y].transform.childCount; i++)
                {
                    if (mapObjects[x, y].transform.GetChild(i).gameObject.activeInHierarchy)
                    {
                        mapObjects[x, y].transform.GetChild(i).transform.Translate(planeOffset);
                    }
                }
            }

            pos.z = 0;
            pos.x += 0.5f;
        }

    }
    private void RepositionBlock()
    {

    }

    private GameObject InstantiateBlock(Vector3 pos, float height)
    {
        GameObject cube;

        cube = Instantiate<GameObject>(lightDirt, pos, Quaternion.identity, mapRoot.transform);

        cube.transform.GetChild(1).gameObject.SetActive(height >= GrassHeight && height < SnowHeight); // grass
        cube.transform.GetChild(2).gameObject.SetActive(height < WaterHeight); // water
        cube.transform.GetChild(3).gameObject.SetActive(height >= WaterHeight && height < GrassHeight); //sand
        cube.transform.GetChild(4).gameObject.SetActive(height >= SnowHeight); //snow

        cube.transform.localScale = new Vector3(1, height + 0.1f, 1);
        cube.transform.position = new Vector3(pos.x, pos.y /*+ (height * 0.5f)*/, pos.z);
        for (int i = 1; i < cube.transform.childCount; i++)
        {
            if (cube.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                cube.transform.GetChild(i).transform.Translate(planeOffset);
            }
        }



        return cube;
    }
}
