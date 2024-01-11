using System.Diagnostics;
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
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public bool isCrouching = false;
    public float playerStamina;

    public bool dash_avail = true;
    public float rec_time = 3f;
    public float gravity = +9.8f;
    public Vector3 fallvelocity; 

    public bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float gcheck_radius = 0.4f;
    public float jumpHeight = 3f;
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
        isGrounded = Physics.CheckSphere(groundCheck.position,gcheck_radius,groundLayer);
        if(isGrounded  && fallvelocity.y <  0f)
        {
            fallvelocity.y = -2f;
        }
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
        else if (isCrouching && playerStamina > 0f)
        {
             Pmoment(momentSpeed - 5f,playermove);
             playerStamina += 2f * Time.deltaTime;
             playerStamina = Mathf.Clamp(playerStamina,-10f,50f);
             state = 1;

        }
        else
        {
            Pmoment(momentSpeed,playermove);
             playerStamina += 1f * Time.deltaTime;
             playerStamina = Mathf.Clamp(playerStamina,-10f,50f);
             state = 3;
        }

        if((Input.GetKeyDown(KeyCode.LeftControl) && x != 0f )||( y != 0f && Input.GetKeyDown(KeyCode.LeftControl)))
        {
            Dashing();
        }

        fallvelocity.y += gravity * Time.deltaTime;

        charcontroller.Move( Time.deltaTime * fallvelocity);
        
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            fallvelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity );
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            if(isCrouching)
            {
                isCrouching = false;
                charcontroller.height = standHeight;
                state = 0;
            }
            else
            {
                isCrouching = true;
                charcontroller.height = crouchHeight;
                state = 1;
            }
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
            //Debug.Log("dashes");
            Pmoment(momentSpeed + 800f,playermove);
            dash_avail = false;
            StartCoroutine(Colldown());

        }

    }

    public IEnumerator Colldown()
    {
        yield return new WaitForSeconds(rec_time);
        dash_avail = true;
        //Debug.Log("skill availabele now");
    }

}
