using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Random = Unity.Mathematics.Random;
using System;

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
    public Transform armoryref;
    private AudioSource audioSource;
    [Tooltip("this is the silende one")]
    public AudioClip audioClip1;
    public AudioClip audioClip2;
    [HideInInspector]
    public AudioClip audioClip;
    public int currentAmmo ;
    public int maxAmmo;
    public float reloadTime;
    private bool isReloading = false;
    
    public Transform fpscamera;
    private armory stats;
    private MouseLook mouseLook;
    public int ammo_type = 0;
    public int maxStack;
    public bool silenced;

    public float recoilx;
    public float recoily;
    public Transform uieffect;
    //1 for 5.3
    //2 for 7.5
    // Start is called before the first frame update
 
   

    // Start is called before the first frame update
    public void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if(silenced)
        {
            audioClip = audioClip1;
        }
        else
        {
            audioClip = audioClip2;
        }
        stats = armoryref.GetComponent<armory>();
        if (stats.ammo5_3 >= maxAmmo)
        {
            currentAmmo = maxAmmo;
            stats.ammo5_3 -= maxAmmo;
        }
        else if (stats.ammo5_3 < maxAmmo)
        {
            currentAmmo = stats.ammo5_3;
            stats.ammo5_3 = 0;
        }
        mouseLook = fpscamera.GetComponent<MouseLook>();
        Debug.Log(mouseLook.xRotation);
    }
     void OnEnable()
    {
        isReloading = false;    
    }

    // Update is called once per frame
    void Update()
    {
     if(isReloading)
        return;
     if(Input.GetKey(KeyCode.Mouse0) && !isShooting && automode && stats.canhoot)
     {
        StartCoroutine(Shootspecfic());
     }
     else if(Input.GetKeyDown(KeyCode.Mouse0) && !automode && stats.canhoot)
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
     if((Input.GetKeyDown(KeyCode.R) && !isReloading && stats.ammo5_3 > 0)||(!isReloading && currentAmmo == 0 && stats.ammo5_3 > 0) )
     {
        StartCoroutine(Reloading());
     }
     else if(Input.GetKeyDown(KeyCode.T) && !isReloading && stats.ammo5_3 > 0)
     {
        StartCoroutine(SlowReloading());
     }
        
    }
    public void RecoilPass()
    {
        float xrecoil = ((UnityEngine.Random.value - 0.5f)/2 ) * recoilx;
        float yrecoil = ((UnityEngine.Random.value - 0.5f)/2) * recoily;

        mouseLook.xRotation -= xrecoil;
        mouseLook.yRotaion -= (yrecoil);

        
    }
     IEnumerator SlowReloading()
    {
        isReloading = true ;
        yield return new WaitForSeconds(reloadTime + 5);
        if((stats.ammo5_3 + currentAmmo)>= maxAmmo)
        {
            stats.ammo5_3 += currentAmmo;
            currentAmmo = maxAmmo;
            stats.ammo5_3 -= maxAmmo;
        }
        else if((stats.ammo5_3 + currentAmmo )< maxAmmo)
        {   

            currentAmmo += stats.ammo5_3;
            stats.ammo5_3 = 0;
        }
        else
        {
            Debug.Log("sometign went wrong");
        }
        isReloading = false;
    }
    IEnumerator Reloading()
    {
        isReloading = true ;
        yield return new WaitForSeconds(reloadTime);
        if(stats.ammo5_3 >= maxAmmo)
        {
            currentAmmo = maxAmmo;
            stats.ammo5_3 -= maxAmmo;
        }
        else if(stats.ammo5_3 < maxAmmo)
        {
            currentAmmo = stats.ammo5_3;
            stats.ammo5_3 = 0;
        }
        else
        {
            Debug.Log("sometign went wrong");
        }
        isReloading = false;
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
        if(currentAmmo > 0)
        {
         muzzleflash.Play();
         RaycastHit hit;
         currentAmmo --;
         audioSource.PlayOneShot(audioClip);
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
            StartCoroutine(UIplay());
            Destroy(flarego,2f);

            
       }
       else
       {
        Debug.Log("nothing");
       }
       RecoilPass(); 
            
        }
        
       
        
    }
    public IEnumerator UIplay()
    {
        uieffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f/fireRate);
        uieffect.gameObject.SetActive(false);
    }
}
