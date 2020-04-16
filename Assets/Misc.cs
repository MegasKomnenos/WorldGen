﻿  
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

    public enum TileTerrainType 
    {  
        Mountain,
        Hill,
        Plain,
        WaterShallow,
        WaterDeep
    }

    public enum TileFeatureType
    {
        None,
        Forest,
        Swamp
    }
}