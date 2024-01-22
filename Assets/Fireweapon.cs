




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
    public Vector3 defaultLocation;
    public Vector3 aimdownPosition1;
    public Vector3 aimdownPosition2;
    public Vector3 aim2rotate;
    public Vector3 defaultAim;
    Vector3 defaultAimRotation;
    public float aimdownSpeed;
    public int aimmode = 0;
    public float aimfov;
    public float defaultfov;

    public bool isAiming;

    //recoill
    [SerializeField]public AnimationCurve kickbackCurve = AnimationCurve.EaseInOut(0f, 0f, 0f, 0f);
    public float kickbackforce;
    private NewCameraRecoil Recoil_Script;
    [SerializeField]private Transform cameraRecoil;
    float kickbackAmount;
    Vector3 originalPosition;
    
    //1 for 5.3
    //2 for 7.5
    // Start is called before the first frame update
 


    // Start is called before the first frame update
    public void Start()
    { 
        originalPosition = transform.localPosition;
        //camerarecoil
        Recoil_Script = cameraRecoil.GetComponent<NewCameraRecoil>();
        defaultfov = (fpscamera.GetComponent<Camera>()).focalLength;
        defaultAim = transform.localPosition;
        audioSource = gameObject.GetComponent<AudioSource>();
        defaultAimRotation = transform.localRotation.eulerAngles;
        Debug.Log(defaultfov);
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
     void OnDisable()
    {
        ReturnToDefault(0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
     {  
        if(aimmode == 0)
        {
            aimmode = 1;
            if(isAiming)
            { 
                 AimDownSight(aimdownPosition2,aim2rotate,aimdownSpeed,aimfov);
            }
        }
        else
        {
            aimmode = 0;
            if(isAiming)
            {
                AimDownSight(aimdownPosition1,defaultAimRotation,aimdownSpeed,aimfov);
            }

        }

     }
     if(Input.GetMouseButtonDown(1) )
     {
        ToggleAimDownSights();
     }
        
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
    private IEnumerator ApplyKickback()
    {
        
        float elapsedTime = 0f;
   
        float maxKickback = kickbackCurve.Evaluate(1f);

        while (elapsedTime < .4f)
        {
            if(isAiming)
            {
                kickbackAmount = kickbackCurve.Evaluate(elapsedTime);
                transform.localPosition = originalPosition - kickbackAmount * (kickbackforce + 25f) * transform.forward;

                elapsedTime += Time.deltaTime / kickbackforce;
                yield return null;

            }
            else if(!isAiming)
            {
                kickbackAmount = kickbackCurve.Evaluate(elapsedTime);
                transform.localPosition = aimdownPosition1 - kickbackAmount * kickbackforce * transform.forward;

                elapsedTime += Time.deltaTime / kickbackforce;
                yield return null;
            }
           
        }

    // Ensure it reaches exactly the original position
        transform.localPosition = originalPosition;
    }
    void ToggleAimDownSights()
    {
        isAiming = !isAiming;

        if (isAiming && aimmode  == 0)
        {
            AimDownSight(aimdownPosition1,defaultAimRotation,aimdownSpeed * Time.deltaTime ,aimfov);
        }
        else if (isAiming && aimmode  == 1)
        {
            AimDownSight(aimdownPosition2,aim2rotate,aimdownSpeed * Time.deltaTime ,aimfov);
        }
        else
        {
            ReturnToDefault(aimdownSpeed * Time.deltaTime);
        }
    }
    void AimDownSight(Vector3 newpositon,Vector3 rr,float t,float fov)
    {
        isAiming = true;
        transform.localPosition = Vector3.Lerp(defaultAim,newpositon,Mathf.Clamp01(t));
        
        transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(rr),Mathf.Clamp01(t));
        (fpscamera.GetComponent<Camera>()).focalLength = fov;
        

    }
    void ReturnToDefault(float t)
    {
        isAiming = false;
        transform.localPosition = Vector3.Lerp(transform.localPosition,defaultAim,Mathf.Clamp01(t));
        transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(defaultAimRotation),Mathf.Clamp01(t));
        (fpscamera.GetComponent<Camera>()).focalLength = defaultfov;
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
        StartCoroutine(ApplyKickback());

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
       //RecoilPass(); 
       Recoil_Script.RecoilFire();     
        }
        
       
        
    }
    public IEnumerator UIplay()
    {
        uieffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f/fireRate);
        uieffect.gameObject.SetActive(false);
    }
}
