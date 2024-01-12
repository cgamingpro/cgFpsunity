using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Fireweapon : MonoBehaviour
{
    [SerializeField]
    float damage = 10f;
    float range = 100f;
    public ParticleSystem muzzleflash;
    public GameObject hitpoint;
    [SerializeField]
    float bulletImpact = 15f;
    [SerializeField]
    float fireRate = 20f;
    bool isShooting = false;
    bool automode = true;

    public Transform fpscamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKey(KeyCode.Mouse0) && !isShooting && automode)
     {
        StartCoroutine(Shootspecfic());
     }
     else if(Input.GetKeyDown(KeyCode.Mouse0) && !automode)
     {
        Shoot();
     }   

     if(Input.GetKeyDown(KeyCode.X) && automode)
     {
        automode = false;
     }   
     else if(Input.GetKeyDown(KeyCode.X) && !automode)
     {
        automode = true;
     }
        
    }
    
    public IEnumerator Shootspecfic()
    {
        isShooting = true;
        while(Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
            yield return new WaitForSeconds((1f/fireRate));

        }
        isShooting = false;

    }
    public void Shoot()
    {   
        muzzleflash.Play();
        RaycastHit hit;
       
       if(Physics.Raycast(fpscamera.transform.position,fpscamera.transform.forward,out hit,range))
       {
        T_damage objhitted =  hit.transform.GetComponent<T_damage>();
        if(objhitted != null)
        {
            
            objhitted.DoDamage(damage);
        }
        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-hit.normal * bulletImpact );
        }

        GameObject flarego =  Instantiate(hitpoint,hit.point,Quaternion.LookRotation(hit.normal));
        Destroy(flarego,2f);

       }
       else
       {
        Debug.Log("nothing");
       }
        
       
        
    }
}
