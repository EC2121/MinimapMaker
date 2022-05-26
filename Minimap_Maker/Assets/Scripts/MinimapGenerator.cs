using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Biome {Ocean,Plain,Mountain,Mixed};
public enum FadeType { Center,LeftToRight,RightToLeft,TopToBottom,BottomToTop,Corner,Random,NoiseMap};
public class MinimapGenerator : MonoBehaviour
{
    [HideInInspector]
    public Biome BiomeSelector = Biome.Mixed;
    [HideInInspector]
    public FadeType FadeType = FadeType.NoiseMap;

    public bool RandomBiome;
    public bool RandomFade;

  
    private int width;
    private int height;
    private Dictionary<Biome, MapData> biomesData;
    private GameObject defaultCube;
   
    private GameObject mapRoot;
    private Texture2D noiseTexture;
    public static UnityEvent<Texture2D,Biome,FadeType> ChangeMap = new UnityEvent<Texture2D,Biome,FadeType>();

    
    void Start()
    {
        MapData mixedMapData = Resources.Load<MapData>("MapDataDefault");
        MapData plainsMapData = Resources.Load<MapData>("MapDataPlain");
        MapData oceanMapData = Resources.Load<MapData>("MapDataOcean");
        MapData mountainsMapData = Resources.Load<MapData>("MapDataMountain");
        biomesData = new Dictionary<Biome, MapData>();
        biomesData[Biome.Mixed] = mixedMapData;
        biomesData[Biome.Ocean] = oceanMapData;
        biomesData[Biome.Mountain] = mountainsMapData;
        biomesData[Biome.Plain] = plainsMapData;

        mapRoot = GameObject.Find("MapRoot");
        defaultCube = Resources.Load<GameObject>("Cube");
       
        width = mixedMapData.Width;
        height = mixedMapData.Height;
       
        GenerateTexture();
        SpawnMap();
    }

    public void GenerateTexture()
    {
        noiseTexture = new Texture2D(width, height);
        System.Random rand = new System.Random(System.DateTime.Now.Millisecond);

        int randOffSet = rand.Next(1, 100);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = 1 + (float)x / width * 2;
                float yCoord = 1 + (float)y / height * 2;

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
        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {
                pos.z += 0.5f;
                InstantiateBlock(pos,x,y);
            }
            pos.z = 0;
            pos.x += 0.5f;
        }
    }

    public void RepositionMap()
    {
        if (RandomBiome)
            BiomeSelector = (Biome)UnityEngine.Random.Range(0, (int)Biome.Mixed + 1);
        if (RandomFade)
            FadeType = (FadeType)UnityEngine.Random.Range(0, (int)FadeType.NoiseMap + 1);

        ChangeMap?.Invoke(noiseTexture,BiomeSelector,FadeType);
    }
 

    private GameObject InstantiateBlock(Vector3 pos,int indexX,int indexY)
    {
        GameObject cube;
        cube = Instantiate<GameObject>(defaultCube, pos, Quaternion.identity, mapRoot.transform);
        MapCube mapCube = cube.GetComponent<MapCube>();
        mapCube.Load(biomesData,indexX,indexY);
        cube.transform.position = new Vector3(pos.x, pos.y /*+ (height * 0.5f)*/, pos.z);
        //cube.transform.localScale = new Vector3(1, height + 0.1f, 1);
        return cube;
    }
}
