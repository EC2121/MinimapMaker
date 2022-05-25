using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCube : MonoBehaviour
{
    // Start is called before the first frame update
    public int indexX;
    public int indexY;
    private float waitForSeconds;
    private bool Changing;
    private Vector3 newHeight = new Vector3(1,0,1);
    private Vector3 planeOffset = new Vector3(0, 0.01f, 0);

    private void Awake()
    {
        MinimapGenerator.ChangeMap.AddListener(StartRepositioning);
    }

    public void StartRepositioning(Texture2D noiseTexutre)
    {
        Changing = true;
        waitForSeconds = (Mathf.Abs(16 - (indexX) ) + Mathf.Abs(16 - indexY)) * 0.05f;
        newHeight.y = noiseTexutre.GetPixel(indexX, indexY).r * MinimapGenerator.MaxHeight;

      
        transform.GetChild(1).gameObject.SetActive(newHeight.y >= MinimapGenerator.GrassHeight && newHeight.y < MinimapGenerator.SnowHeight); // grass
        transform.GetChild(2).gameObject.SetActive(newHeight.y < MinimapGenerator.WaterHeight); // water
        transform.GetChild(3).gameObject.SetActive(newHeight.y >= MinimapGenerator.WaterHeight && newHeight.y < MinimapGenerator.GrassHeight); //sand
        transform.GetChild(4).gameObject.SetActive(newHeight.y >= MinimapGenerator.SnowHeight); //snow
        for (int i = 1; i < transform.childCount; i++)
        {
           
                transform.GetChild(i).transform.Translate(-planeOffset);
        }

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
                        
                            transform.GetChild(i).transform.Translate(planeOffset);
                    }
                }
            }
        }
    }
    

}
