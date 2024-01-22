using System.Collections;
using UnityEngine;

public class NewCameraRecoil : MonoBehaviour
{   //rotations
    private Vector3 currentRotaion;
    private Vector3 targetRotation;

    //recoil cordivates 

    public float recoilX;
    public float recoilY;
    public float recoilZ;
    
    //recoil control;
    [SerializeField]private  float snappiness;
    [SerializeField]private float returnSpeed;

    //weaponkickback
    private weaponswitching weaponswitching;
    private int weaponIndex;
    private Transform RifleSelected;
    public float weaponKickbackForce;
    [SerializeField] float weaponDamping;
    Vector3 defaultRifle;
    // Start is called before the first frame update
    void Start()
    {
       weaponswitching =  transform.GetChild(0).GetChild(0).GetComponent<weaponswitching>();
       
     
       

    }

    // Update is called once per frame
    void Update()
    {
        weaponIndex = weaponswitching.selectedWeapon;
        RifleSelected = weaponswitching.transform.GetChild(weaponIndex);
        targetRotation = Vector3.Lerp(targetRotation,Vector3.zero,returnSpeed * Time.deltaTime);
        currentRotaion = Vector3.Slerp(currentRotaion,targetRotation,snappiness * Time.deltaTime);
       
        
        transform.localRotation = Quaternion.Euler(currentRotaion);
    }
    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX,Random.Range(-recoilY,recoilY),Random.Range(-recoilZ,recoilZ));
        RifleSelected.transform.Translate(Vector3.back * weaponKickbackForce ,Space.Self);

        
    }

}