using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed;
    public float scrollSpeed;
    
    private Vector3 movementHistory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var transform = GetComponent<Transform>();

        if (Input.GetMouseButton(2))
        {
            movementHistory *= 0.5f; 
            movementHistory += new Vector3(
                Input.GetAxis("Mouse X") * movementSpeed * Mathf.Sqrt(Mathf.Abs(transform.position.z)) * Time.deltaTime, 
                Input.GetAxis("Mouse Y") * movementSpeed * Mathf.Sqrt(Mathf.Abs(transform.position.z)) * Time.deltaTime, 0);

            transform.position -= movementHistory;
        }
        else
        {
            movementHistory *= 0;

            transform.position += new Vector3(0, 0, Input.mouseScrollDelta.y * Mathf.Sqrt(Mathf.Abs(transform.position.z)) * scrollSpeed);
        }
    }
}
