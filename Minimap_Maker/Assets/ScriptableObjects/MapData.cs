using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapData : ScriptableObject
{
    public int Width;
    public int Height;

    public int MaxHeight = 15;
    public int GrassHeight = 5;
    public int SandHeight = 4;
    public int WaterHeight = 3;
    public int SnowHeight = 13;
}
