using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Swaying : MonoBehaviour
{
    public float swayMuliplier;
    public float smoothsway;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mousex = Input.GetAxisRaw("Mouse X") * swayMuliplier;
        float mousey = Input.GetAxisRaw("Mouse Y") * swayMuliplier;

        Quaternion rotationX = Quaternion.AngleAxis(mousey,Vector3.left);
        Quaternion rotationY = Quaternion.AngleAxis(mousex,Vector3.up);

        Quaternion desireRotaion = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation,desireRotaion,smoothsway * Time.deltaTime);

        

    }
}
