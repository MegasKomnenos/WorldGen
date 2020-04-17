using System.Collections;
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

    public Camera cameraPlayer;

    private List<Misc.TypeTerrain> listTerrain;

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
        if (Input.GetMouseButtonDown(0))
        {
            var ray = cameraPlayer.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                Debug.Log("Did hit");

                var tile = new Vector3Int(-1, -1, -1);

                var tile0 = grid.WorldToCell(hit.point + new Vector2(0, 5f / 66f));
                var tile1 = grid.WorldToCell(hit.point);
                var tile2 = grid.WorldToCell(hit.point - new Vector2(0, 5f / 66f));
                var tile3 = grid.WorldToCell(hit.point - new Vector2(0, 10f / 66f));

                var i0 = Misc.GetIndexFromCoord(width, height, tile0.x, tile0.y);
                var i1 = Misc.GetIndexFromCoord(width, height, tile1.x, tile1.y);
                var i2 = Misc.GetIndexFromCoord(width, height, tile2.x, tile2.y);
                var i3 = Misc.GetIndexFromCoord(width, height, tile3.x, tile3.y);

                if (i0 != -1 && (listTerrain[i0] == Misc.TypeTerrain.Shallow || listTerrain[i0] == Misc.TypeTerrain.Deep))
                {
                    tile = tile0;
                }
                if (i1 != -1 && listTerrain[i1] == Misc.TypeTerrain.Plain)
                {
                    tile = tile1;
                }
                if (i2 != -1 && listTerrain[i2] == Misc.TypeTerrain.Hill)
                {
                    tile = tile2;
                }
                if (i3 != -1 && listTerrain[i3] == Misc.TypeTerrain.Mountain)
                {
                    tile = tile3;
                }

                Debug.Log(tile);
            }
            else
            {
                Debug.Log("Did not hit");
            }
        }
    }
}