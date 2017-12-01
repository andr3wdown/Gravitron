using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlanetGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public int seed;
    public float noiseScale;
    public int octaves;
    [Range(0.00f, 1.00f)]
    public float persistance;
    public float lacunarity; 
    public Vector2 offset;
    public TerrainType[] regions;
    public string filePath = "/Planets/";
    public string fileName = "planet";
    public int fileIndex = 0;
    public string ext = ".png";
    public Texture2D GenerateImage()
    {
        RandomiseColors();
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        Color[] colorMap = new Color[mapHeight * mapWidth];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }
        Texture2D tex = TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight);
        return tex;
    }
    public void WriteTexture(Texture2D tex)
    {
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + filePath + fileName + fileIndex + ext, bytes);
        fileIndex++;
    }
    public void RandomiseColors()
    {
        noiseScale = Random.Range(4f, 150f);
        offset.x = Random.Range(-10000f, 10000f);
        offset.y = Random.Range(-10000f, 10000f);
        Color r = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        for (int i = 0; i < regions.Length; i++)
        {
            Color c = Color.Lerp(r, new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)), 0.3f);
            regions[i].color = c;
            r = c;
        }
    }
}
