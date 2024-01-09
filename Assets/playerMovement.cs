using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float momentSpeed;
    public CharacterController charcontroller;
    public float sprintSpeed;
    private Vector3 playermove;

    public float playerStamina;

    public bool dash_avail = true;
    public float rec_time = 3f;

    public int state = 0;
    //0 = standing 
    //1 = crouching 
    //2 = runing 
    //3 = walking 
    




    // Start is called before the first frame update
    void Start()
    {
         Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {   
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        playermove = (transform.forward * y) + (transform.right * x);
        if (Input.GetKey("left shift") == true && playerStamina > 0f)
        { 
             Pmoment((momentSpeed + 20f),playermove);
             playerStamina -= 30f * Time.deltaTime;
             playerStamina = Mathf.Clamp(playerStamina,-20f,50f);
             state = 2;
                      
        }
        
        else if ((x == 0 && y ==0))
        {
             playerStamina += 5f * Time.deltaTime;
             playerStamina = Mathf.Clamp(playerStamina,-10f,50f);
             state = 0;
        }
        else
        {
            Pmoment(momentSpeed,playermove);
             playerStamina += 1f * Time.deltaTime;
             playerStamina = Mathf.Clamp(playerStamina,-10f,50f);
             state = 3;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Dashing();
        }





        
    }

    public void Pmoment(float p_speed,Vector3 p_move)
    {
        charcontroller.Move(p_speed * Time.deltaTime * p_move);
            
    }

    public void Dashing()
    {
        if(dash_avail)
        {
            Debug.Log("dashes");
            Pmoment(momentSpeed + 800f,playermove);
            dash_avail = false;
            StartCoroutine(Colldown());

        }
        else
        {
            Debug.Log("not available");
        }
    }

    public IEnumerator Colldown()
    {
        yield return new WaitForSeconds(rec_time);
        dash_avail = true;
        Debug.Log("skill availabele now");
    }

}
