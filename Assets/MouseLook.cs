using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSenstivity;
    public Transform playerBody;
    private float xRotation = 0f ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenstivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSenstivity * Time.deltaTime;
        // xRotation -= mouseY;
        // xRotation = Mathf.Clamp(xRotation,-90f,90f);

        // transform.localRotation = quaternion.Euler(xRotation,0f,0f);
         // Adjust vertical rotation based on mouseY
        xRotation -= mouseY;

        // Apply vertical rotation gradually with smoothing
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation within -90 to 90 degrees
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        
        
    }
}
