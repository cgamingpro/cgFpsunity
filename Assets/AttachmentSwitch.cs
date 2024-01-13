using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

public class AttachmentSwitch : MonoBehaviour
{
    
    [SerializeField]
    private KeyCode yourKeyCode = KeyCode.Alpha1;
    public int Attachment = 0;
    public Transform armoryref;
    private armory stats;
    public bool AttachEdit = false;
    // Start is called before the first frame update
    void Start()
    {
        stats = armoryref.GetComponent<armory>();
        SelectAttachment();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {   
            if(AttachEdit)
                {   
                    AttachEdit = false;
                    stats.canhoot = true;
                }
            else if(!AttachEdit)
                {
                 
                    AttachEdit = true;
                    stats.canhoot  = false;
                }
        }
       
        if(AttachEdit && Input.GetKeyDown(yourKeyCode))
        {
            if(Attachment >= (transform.childCount - 1))
            {
                Attachment = 0;
            }
            else
            {
                Attachment++;
            }
            SelectAttachment();
        }
        
    }
    void SelectAttachment()
    {
        int i = 0;
        foreach(Transform attachments in transform)
        {
            if(i == Attachment)
            {
                attachments.gameObject.SetActive (true);
            }
            else
            {
                attachments.gameObject.SetActive(false);
            }
            i++;
        }
        
    }
}
