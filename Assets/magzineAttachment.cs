using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class magzineAttachment : MonoBehaviour
{
    public Transform weapons;
    private Fireweapon fireweapon;
    int hold;
    // Start is called before the first frame update
    void Start()
    {
        fireweapon = weapons.GetComponent<Fireweapon>();
    }
    void OnEnable()
    {
        if(fireweapon != null)
        {
            hold = fireweapon.maxAmmo;
            fireweapon.maxAmmo = 80;
            Debug.Log(hold);

        }
        {
            Debug.Log("found the erroe");
        }
        
    }
    void OnDisable()
    {
        fireweapon.maxAmmo = hold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
