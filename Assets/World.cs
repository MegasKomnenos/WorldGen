﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    public int width;
    public int height;

    public int detail;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;
    
    public float weightPeak;
    public float weightSea;

    public int seedHeight;

    public Tilemap mapTerrain;
    public Tilemap mapFeature;
    public Grid grid;

    public Tile tileWaterShallow;
    public Tile tileWaterDeep;
    public Tile tileLandPlain;
    public Tile tileLandHill;
    public Tile tileLandMountain;

    public List<Misc.TypeTerrain> listTerrain;

    // Start is called before the first frame update
    void Start()
    {
        var widthPixel = width * Mathf.Sqrt(3);
        var heightPixel = height * 1.5F + 0.5F;

        if ((height + 1) % 2 == 1)
        {
            widthPixel += Mathf.Sqrt(3) / 2;
        }

        var mapElevation = Noise.GenerateNoiseMap(detail, detail, seedHeight, scale, octaves, persistance, lacunarity);
        var listElevation = new List<float>(width * height);

        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                var xCenter = x * Mathf.Sqrt(3) + Mathf.Sqrt(3) / 2;
                var yCenter = y * 1.5F + 1;

                if (y % 2 == 1)
                {
                    xCenter += Mathf.Sqrt(3) / 2;
                }

                listElevation.Add(
                    mapElevation[(int)(xCenter / widthPixel * detail), (int)(yCenter / heightPixel * detail)]
                    * weightPeak
                    - (Mathf.Pow(widthPixel / 2 - xCenter, 2) + Mathf.Pow(heightPixel / 2 - yCenter, 2))
                    / (Mathf.Pow(widthPixel / 2, 2) + Mathf.Pow(heightPixel / 2, 2))
                    * weightSea
                );
            }
        }

        listTerrain = new List<Misc.TypeTerrain>(width * height);

        var i = 0;

        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                if (listElevation[i] >= 0.8)
                {
                    mapTerrain.SetTile(new Vector3Int(x, y, 0), tileLandMountain);
                    listTerrain.Add(Misc.TypeTerrain.Mountain);
                }
                else if (listElevation[i] >= 0.6)
                {
                    mapTerrain.SetTile(new Vector3Int(x, y, 0), tileLandHill);
                    listTerrain.Add(Misc.TypeTerrain.Hill);
                }
                else if (listElevation[i] >= 0.4)
                {
                    mapTerrain.SetTile(new Vector3Int(x, y, 0), tileLandPlain);
                    listTerrain.Add(Misc.TypeTerrain.Plain);
                }
                else if (listElevation[i] >= 0.2)
                {
                    mapTerrain.SetTile(new Vector3Int(x, y, 0), tileWaterShallow);
                    listTerrain.Add(Misc.TypeTerrain.Shallow);
                }
                else 
                {
                    mapTerrain.SetTile(new Vector3Int(x, y, 0), tileWaterDeep);
                    listTerrain.Add(Misc.TypeTerrain.Deep);
                }

                ++i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}