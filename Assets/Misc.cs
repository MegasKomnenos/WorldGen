  
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Noise {
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity) {
		float[,] noiseMap = new float[mapWidth,mapHeight];

		System.Random prng = new System.Random (seed);
		Vector2[] octaveOffsets = new Vector2[octaves];
		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next (-100000, 100000);
			float offsetY = prng.Next (-100000, 100000);
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}

		if (scale <= 0) {
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;


		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
		
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = (x-halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
				}

				if (noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;
				} else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}
				noiseMap [x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
			}
		}

		return noiseMap;
	}
}

public static class Misc
{
    public static List<int> GetHexNeighb(int width, int height, int x, int y)
    {
        var neighb = new List<int>();
        var i = y * width + x;

        if (x + 1 < width)
        {
            neighb.Add(i + 1);
        }
        if (x > 0)
        {
            neighb.Add(i - 1);
        }
        if (y > 0)
        {
            neighb.Add(i - width);

            if (y % 2 == 1)
            {
                if (x + 1 < width)
                {
                    neighb.Add(i - width + 1);
                }
            }
            else if (x > 0)
            {
                neighb.Add(i - width - 1);
            }
        }
        if (y + 1 < height)
        {
            neighb.Add(i + width);

            if (y % 2 == 0)
            {
                if (x > 0)
                {
                    neighb.Add(i + width - 1);
                }
            }
            else if (x + 1 < width)
            {
                neighb.Add(i + width + 1);
            }
        }

        return neighb;
    }

    public static int GetIndexFromCoord(int width, int height, int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return -1;
        }

        return y * width + x;
    }

    public static Vector3Int GetTile(Vector2 point, Grid grid, World world)
    {
        var tile = new Vector3Int(-1, -1, -1);

        var tile0 = grid.WorldToCell(point + new Vector2(0, 5f / 66f));
        var tile1 = grid.WorldToCell(point);
        var tile2 = grid.WorldToCell(point - new Vector2(0, 5f / 66f));
        var tile3 = grid.WorldToCell(point - new Vector2(0, 10f / 66f));

        var i0 = GetIndexFromCoord(world.width, world.height, tile0.x, tile0.y);
        var i1 = GetIndexFromCoord(world.width, world.height, tile1.x, tile1.y);
        var i2 = GetIndexFromCoord(world.width, world.height, tile2.x, tile2.y);
        var i3 = GetIndexFromCoord(world.width, world.height, tile3.x, tile3.y);

        if (i0 != -1 && (world.listTerrain[i0] == TypeTerrain.Shallow || world.listTerrain[i0] == TypeTerrain.Deep))
        {
            tile = tile0;
        }
        if (i1 != -1 && world.listTerrain[i1] == TypeTerrain.Plain)
        {
            tile = tile1;
        }
        if (i2 != -1 && world.listTerrain[i2] == TypeTerrain.Hill)
        {
            tile = tile2;
        }
        if (i3 != -1 && world.listTerrain[i3] == TypeTerrain.Mountain)
        {
            tile = tile3;
        }

        return tile;
    }

    public enum TypeTerrain
    {
        Plain,
        Hill,
        Mountain,
        Shallow,
        Deep
    }
}