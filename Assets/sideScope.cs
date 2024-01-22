
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class sideScope : MonoBehaviour
{
    public Transform rifle;
    private Fireweapon fireweapon;
    public float fov_value;

    [SerializeField]private Vector3 aimModepos;


    // Start is called before the first frame update
    void Start()
    {
        fireweapon = rifle.GetComponent<Fireweapon>(); 
    }
    void OnEnable()
    {
    //     fireweapon.aimdownPosition2 = aimModepos;
    //     fireweapon.aimfov = fov_value;
    //     Debug.Log("side scope is executed");
     if (fireweapon != null)
        {
            fireweapon.aimdownPosition2 = aimModepos;
            fireweapon.aimfov = fov_value;
            Debug.Log("side scope is executed");
        }
        else
        {
            Debug.LogError("Fireweapon component not found on rifle.");
        }
    }

    // Update is called once per frame
    
}
