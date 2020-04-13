using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int heightLand;
    public int heightWater;
    public int heightTotal;

    /*
    public int xInd;
    public int yInd;

    public List<(float, float)> vertices;

    public Tile(int x, int y, float r)
    {
        var xCenter = (2 * x + y % 2 + 1) * r * Mathf.Sqrt(3) / 2;
        var yCenter = ((y + 1) / 2) * 2 * r + (y / 2) * r + r;

        vertices.Add((xCenter - r * Mathf.Sqrt(3) / 2, yCenter - r / 2));
        vertices.Add((xCenter, yCenter - r));
        vertices.Add((xCenter + r * Mathf.Sqrt(3) / 2, yCenter - r / 2));
        vertices.Add((xCenter + r * Mathf.Sqrt(3) / 2, yCenter + r / 2));
        vertices.Add((xCenter, yCenter + r));
        vertices.Add((xCenter - r * Mathf.Sqrt(3), yCenter + r / 2));

        xInd = x;
        yInd = y;
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}