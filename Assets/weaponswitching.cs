using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class weaponswitching : MonoBehaviour
{

    public int selectedWeapon = 1;
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {   
            
            if(selectedWeapon >= (transform.childCount - 1))
            {
                selectedWeapon = 0;
            }
            else
                selectedWeapon++;
            SelectWeapon();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            
            if(selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
                selectedWeapon--;
            SelectWeapon();
        }
    
    }
    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform gunes in transform)
        {
            if(i == selectedWeapon)
                gunes.gameObject.SetActive(true);
            else
                gunes.gameObject.SetActive(false);

            i++;    
        }

    }
}
