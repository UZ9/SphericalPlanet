using System;
using UnityEngine;


//https://github.com/WardBenjamin/SimplexNoise/blob/master/SimplexNoise/Noise.cs
public static class PlanetNoise
{
    public static float Generate(float x, float y, float z)
    {

        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);
        float yx = Mathf.PerlinNoise(y, x);
        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);

        return (xy + xz + yz + yx + zx + zy) / 6;
    }





}