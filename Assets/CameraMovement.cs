using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed;
    public float scrollSpeed;
    public float sizeMin;
    public float sizeMax;
    
    private Vector3 movementHistory;
    private Camera cameraSelf;
    private Transform transformSelf;

    // Start is called before the first frame update
    void Start()
    {
        cameraSelf = GetComponent<Camera>();
        transformSelf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        var scale = Mathf.Sqrt(cameraSelf.orthographicSize);

        if (Input.GetMouseButton(2))
        {
            movementHistory *= 0.5f; 
            movementHistory += new Vector3(
                Input.GetAxis("Mouse X") * movementSpeed * scale * Time.deltaTime, 
                Input.GetAxis("Mouse Y") * movementSpeed * scale * Time.deltaTime, 0);

            transformSelf.position -= movementHistory;
        }
        else
        {
            movementHistory *= 0;

            cameraSelf.orthographicSize -= Input.mouseScrollDelta.y * scale * scrollSpeed;

            if (cameraSelf.orthographicSize < sizeMin)
            {
                cameraSelf.orthographicSize = sizeMin;
            }
            else if (cameraSelf.orthographicSize > sizeMax)
            {
                cameraSelf.orthographicSize = sizeMax;
            }
        }
    }
}
