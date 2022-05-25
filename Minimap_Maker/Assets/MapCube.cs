using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCube : MonoBehaviour
{
    // Start is called before the first frame update
    private int indexX;
    private int indexY;

    private int maxHeight;
    private int grassHeight;
    private int sandHeight;
    private int waterHeight;
    private int snowHeight;

    private int halfWidth;
    private int halfHeight;
    private float waitForSeconds;
    private bool Changing;
    private Vector3 newHeight = new Vector3(1,0,1);
    private Vector3 planeOffset = new Vector3(0, 0.5003f, 0);

    private void Awake()
    {
        MinimapGenerator.ChangeMap.AddListener(StartRepositioning);
    }


    public void Load(MapData data,int indexX,int indexY)
    {
        //transform.GetChild(1).gameObject.SetActive(height >= GrassHeight && height < SnowHeight); // grass
        //transform.GetChild(2).gameObject.SetActive(height < WaterHeight); // water
        //transform.GetChild(3).gameObject.SetActive(height >= WaterHeight && height < GrassHeight); //sand
        //transform.GetChild(4).gameObject.SetActive(height >= SnowHeight); //snow
        transform.GetChild(1).gameObject.SetActive(false); // grass
        transform.GetChild(2).gameObject.SetActive(false); // water
        transform.GetChild(3).gameObject.SetActive(false); //sand
        transform.GetChild(4).gameObject.SetActive(false); //snow
        halfHeight = data.Height / 2;
        halfWidth = data.Width / 2;
        maxHeight = data.MaxHeight;
        grassHeight = data.GrassHeight; 
        waterHeight = data.WaterHeight;
        sandHeight = data.SandHeight;
        snowHeight = data.SnowHeight;
        this.indexX = indexX;
        this.indexY = indexY;
        for (int i = 1; i < transform.childCount; i++)
        {

            transform.GetChild(i).transform.localPosition = planeOffset;
        }
    }
    public void StartRepositioning(Texture2D noiseTexutre)
    {
        Changing = true;
        waitForSeconds = (Mathf.Abs(halfWidth - (indexX) ) + Mathf.Abs(halfHeight - indexY)) * 0.05f;
        newHeight.y = noiseTexutre.GetPixel(indexX, indexY).r * maxHeight;
        StartCoroutine(ChangeTypeCorutine());
        //for (int i = 1; i < transform.childCount; i++)
        //{
           
        //        transform.GetChild(i).transform.Translate(-planeOffset);
        //}
    }


    IEnumerator ChangeTypeCorutine()
    {
        yield return new WaitForSeconds(waitForSeconds);
        transform.GetChild(1).gameObject.SetActive(newHeight.y >= grassHeight && newHeight.y < snowHeight); // grass
        transform.GetChild(2).gameObject.SetActive(newHeight.y < waterHeight); // water
        transform.GetChild(3).gameObject.SetActive(newHeight.y >= waterHeight && newHeight.y < grassHeight); //sand
        transform.GetChild(4).gameObject.SetActive(newHeight.y >= snowHeight); //snow
    }
    private void Update()
    {
        if (Changing)
        {
            waitForSeconds -= Time.deltaTime;
            if (waitForSeconds <= 0)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, newHeight, Time.deltaTime * 3);

                if (Mathf.Abs(transform.localScale.y - newHeight.y) < 0.1f)
                {
                    Changing = false;
                    for (int i = 1; i < transform.childCount; i++)
                    {
                            transform.GetChild(i).transform.localPosition = planeOffset;
                    }
                }
            }
        }
    }
    

}
