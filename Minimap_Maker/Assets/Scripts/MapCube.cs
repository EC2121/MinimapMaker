using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MapCube : MonoBehaviour
{
    // Start is called before the first frame update
    private int indexX;
    private int indexY;

    private float maxHeight;
    private float grassHeight;
    private float sandHeight;
    private float waterHeight;
    private float snowHeight;

    private bool hasTree;
    private bool hasHouse;
    private Transform dirtParent;
    private Transform treeTransform;
    private GameObject house;
    private Dictionary<Biome, MapData> biomeDictionary = new Dictionary<Biome, MapData>();
    private Dictionary<FadeType, float> secondsToWaitDictonary = new Dictionary<FadeType, float>();
    private Vector3 newHeight = new Vector3(1, 0, 1);
    private Vector3 newObjectPos = new Vector3(0, 0, 0);
    private Vector3 planeOffset = new Vector3(0, 0.504f, 0);
    private Biome currentBiome;
    private FadeType currentFadeType;
    private void Awake()
    {
        MinimapGenerator.ChangeMap.AddListener(StartRepositioning);
    }
    private void OnDisable()
    {
        MinimapGenerator.ChangeMap.RemoveListener(StartRepositioning);
    }
    public void Load(Dictionary<Biome, MapData> Dict, int indexX, int indexY)
    {
       
        biomeDictionary = Dict;
        dirtParent = transform.GetChild(0);
        dirtParent.GetChild(1).gameObject.SetActive(false); // grass
        dirtParent.GetChild(2).gameObject.SetActive(false); // water
        dirtParent.GetChild(3).gameObject.SetActive(false); //sand
        dirtParent.GetChild(4).gameObject.SetActive(false); //snow
        transform.GetChild(1).gameObject.SetActive(false); // tree 
        treeTransform = transform.GetChild(1);
     
        this.indexX = indexX;
        this.indexY = indexY;
        for (int i = 1; i < dirtParent.childCount; i++)
        {
            dirtParent.GetChild(i).transform.localPosition = planeOffset;
        }
        if (Random.Range(0, 100) >= 99)
        {
            hasHouse = true;
            GameObject go = Resources.Load<GameObject>("House");
            house = Instantiate<GameObject>(go, transform);
            house.transform.position = transform.position;
        }
        secondsToWaitDictonary[FadeType.Center] = (Mathf.Abs((biomeDictionary[0].Width / 2) - (indexX))
            + Mathf.Abs((biomeDictionary[0].Height / 2) - indexY)) * 0.05f;
        secondsToWaitDictonary[FadeType.LeftToRight] = indexX * 0.05f;
        secondsToWaitDictonary[FadeType.RightToLeft] = (biomeDictionary[0].Width - indexX) * 0.05f;
        secondsToWaitDictonary[FadeType.TopToBottom] = (biomeDictionary[0].Height - indexY) * 0.05f;
        secondsToWaitDictonary[FadeType.BottomToTop] = indexY * 0.05f;
        secondsToWaitDictonary[FadeType.Corner] = (indexX + indexY) * 0.05f;
        secondsToWaitDictonary[FadeType.Random] = UnityEngine.Random.Range(0, 33) * 0.05f;
     
    }
    public void StartRepositioning(Texture2D noiseTexutre, Biome current, FadeType fadeType)
    {

        if (currentBiome != current)
        {
            maxHeight = biomeDictionary[current].MaxHeight;
            grassHeight = biomeDictionary[current].GrassHeight;
            waterHeight = biomeDictionary[current].WaterHeight;
            sandHeight = biomeDictionary[current].SandHeight;
            snowHeight = biomeDictionary[current].SnowHeight;
        }
        currentBiome = current;
        currentFadeType = fadeType;
        
        float pixelColor = noiseTexutre.GetPixel(indexX, indexY).r;

        if (fadeType == FadeType.NoiseMap)
            secondsToWaitDictonary[FadeType.NoiseMap] = (pixelColor * 32) * 0.1f;

        newHeight.y = pixelColor * maxHeight + biomeDictionary[current].MinHeight;

        hasTree = !hasHouse &&
        newHeight.y >= grassHeight &&
            (newHeight.y < snowHeight || newHeight.y >= snowHeight) &&
                Random.Range(0, 100) >= 95;

        newObjectPos = new Vector3(0, newHeight.y * 0.5f, 0);

        StartCoroutine(ChangeTypeCorutine());

    }


    IEnumerator ChangeTypeCorutine()
    {
        yield return new WaitForSeconds(secondsToWaitDictonary[currentFadeType]);
        dirtParent.DOScaleY(newHeight.y + 0.1f, 2f);
        dirtParent.GetChild(1).gameObject.SetActive(newHeight.y >= grassHeight && newHeight.y < snowHeight); // grass
        dirtParent.GetChild(2).gameObject.SetActive(newHeight.y < waterHeight); // water
        dirtParent.GetChild(3).gameObject.SetActive(newHeight.y >= waterHeight && newHeight.y < grassHeight); //sand
        dirtParent.GetChild(4).gameObject.SetActive(newHeight.y >= snowHeight); //snow
        house?.transform.DOMoveY(newHeight.y < waterHeight ? 0 : newObjectPos.y, 2f);
        if (hasTree)
            treeTransform.DOMoveY(newObjectPos.y, 2f);
        else
            treeTransform.DOMoveY(0, 1f);
        yield return new WaitForSeconds(0.5f);
        house?.gameObject.SetActive(!(newHeight.y < waterHeight));
        transform.GetChild(1).gameObject.SetActive(hasTree);

    }


}
