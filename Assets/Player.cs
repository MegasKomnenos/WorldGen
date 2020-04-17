using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public World world;
    public Grid grid;
    public Camera cameraMain;
    public Canvas canvasUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = cameraMain.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                Debug.Log("Did hit");

                var tile = Misc.GetTile(hit.point, grid, world);

                Debug.Log(tile);
            }
            else
            {
                Debug.Log("Did not hit");
            }
        }
    }
}
