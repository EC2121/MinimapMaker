using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapData : ScriptableObject
{
    public int Width;
    public int Height;

    public float MaxHeight = 15;
    public float MinHeight = 15;
    public float GrassHeight = 5;
    public float SandHeight = 4;
    public float WaterHeight = 3;
    public float SnowHeight = 13;
}
