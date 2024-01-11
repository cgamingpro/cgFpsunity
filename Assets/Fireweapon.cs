using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Fireweapon : MonoBehaviour
{

    float damage = 10f;
    float range = 100f;
    public ParticleSystem muzzleflash;
    public GameObject hitpoint;

    public Transform fpscamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.Mouse0))
     {
        Shoot();
     }   
        
        
    }
    public void Shoot()
    {   
        muzzleflash.Play();
        RaycastHit hit;
        Physics.Raycast(fpscamera.transform.position,fpscamera.transform.forward,out hit,range);
        Debug.Log(hit.transform.name);
        T_damage objhitted =  hit.transform.GetComponent<T_damage>();
        if(objhitted != null)
        {
            
            objhitted.DoDamage(damage);
        }

        Instantiate(hitpoint,hit.point,Quaternion.LookRotation(hit.normal));
    }
}
