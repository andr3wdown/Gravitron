using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        colorMap,
        noiseMap,
        Mesh
    }

    public DrawMode drawMode;

    public const int mapChunkSize = 241;

    [Range(0,6)]
    public int levelOfDetail;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int octaves;
    [Range(0.00f,1.00f)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;
    public void GenerateMap()
    {
        mapWidth = mapChunkSize;
        mapHeight = mapChunkSize;

        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);


        Color[] colorMap = new Color[mapHeight * mapWidth];

        for(int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x,y];
                for(int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }

            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.noiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }       
        else if (drawMode == DrawMode.colorMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }          
        else if(drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }
    }
    public string filePath = "/Planets/";
    public string fileName = "planet";
    public int fileIndex = 0;
    public string ext = ".png";
    public void GenerateImage()
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
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + filePath + fileName + fileIndex + ext, bytes);
        fileIndex++;
    }
    public void RandomiseColors()
    {
        noiseScale = Random.Range(4f, 150f);
        offset.x = Random.Range(-10000f, 10000f);
        offset.y = Random.Range(-10000f, 10000f);
        for (int i = 0; i < regions.Length; i++)
        {
            regions[i].color = new Color(Random.Range(0.0f,1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }
    }
    void OnValidate()
    {
        /*
     if(mapWidth < 1)
        {
            mapWidth = 1;
        }

     if(mapHeight < 1)
        {
            mapHeight = 1;
        } */

     if(lacunarity < 1)
        {
            lacunarity = 1;
        }

     if(octaves < 0)
        {
            octaves = 0;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
