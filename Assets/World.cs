using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int seedSwamp;

    public GameObject prefabWaterShallow;
    public GameObject prefabWaterDeep;
    public GameObject prefabLandPlain;
    public GameObject prefabLandHill;
    public GameObject prefabLandMountain;

    public GameObject prefabForest;
    public GameObject prefabSwamp;

    private List<GameObject> listTile;

    // Start is called before the first frame update
    void Start()
    {
        var widthPixel = width * Mathf.Sqrt(3);
        var heightPixel = height * 1.5F + 0.5F;

        if ((height + 1) % 2 == 1)
        {
            widthPixel += Mathf.Sqrt(3) / 2;
        }

        var mapHeight = Noise.GenerateNoiseMap(detail, detail, seedHeight, scale, octaves, persistance, lacunarity);
        var mapSwamp = Noise.GenerateNoiseMap(detail, detail, seedSwamp, scale, octaves, persistance, lacunarity);

        var listElevation = new List<float>(width * height);
        var listFeature = new List<float>(width * height);

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
                    mapHeight[(int)(xCenter / widthPixel * detail), (int)(yCenter / heightPixel * detail)]
                    * weightPeak
                    - (Mathf.Pow(widthPixel / 2 - xCenter, 2) + Mathf.Pow(heightPixel / 2 - yCenter, 2))
                    / (Mathf.Pow(widthPixel / 2, 2) + Mathf.Pow(heightPixel / 2, 2))
                    * weightSea
                );
                listFeature.Add(
                    mapSwamp[(int)(xCenter / widthPixel * detail), (int)(yCenter / heightPixel * detail)]
                    - listElevation[listElevation.Count - 1] / 2
                );
            }
        }

        listTile = new List<GameObject>(width * height);

        float xChange = Mathf.Sqrt(3);
        float xOff = xChange / 2;
        float yChange = 1.5F;
        var i = 0;

        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                if (listElevation[i] >= 0.8)
                {
                    listTile.Add(Instantiate(prefabLandMountain, new Vector3(xChange * x + y % 2 * xOff, 0, yChange * y), Quaternion.identity));
                }
                else if (listElevation[i] >= 0.6)
                {
                    listTile.Add(Instantiate(prefabLandHill, new Vector3(xChange * x + y % 2 * xOff, 0, yChange * y), Quaternion.identity));
                }
                else if (listElevation[i] >= 0.4)
                {
                    listTile.Add(Instantiate(prefabLandPlain, new Vector3(xChange * x + y % 2 * xOff, 0, yChange * y), Quaternion.identity));
                }
                else if (listElevation[i] >= 0.2)
                {
                    listTile.Add(Instantiate(prefabWaterShallow, new Vector3(xChange * x + y % 2 * xOff, 0, yChange * y), Quaternion.identity));
                }
                else 
                {
                    listTile.Add(Instantiate(prefabWaterDeep, new Vector3(xChange * x + y % 2 * xOff, 0, yChange * y), Quaternion.identity));
                }

                var tile = listTile[i].GetComponent<Tile>();

                tile.neighb = Misc.GetHexNeighb(width, height, x, y);

                if (tile.terrain != Misc.TileTerrainType.WaterShallow && tile.terrain != Misc.TileTerrainType.WaterDeep)
                {
                    if (listFeature[i] >= 0.5)
                    {
                        tile.feature = Misc.TileFeatureType.Swamp;
                        tile.featureObject = Instantiate(prefabSwamp, listTile[i].GetComponent<Transform>());
                    }
                    else 
                    {
                        tile.feature = Misc.TileFeatureType.Forest;
                        tile.featureObject = Instantiate(prefabForest, listTile[i].GetComponent<Transform>());
                    }
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