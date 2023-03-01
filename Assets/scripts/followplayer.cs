using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//using UnityEngine.InputSystem;
using UnityEngine.UI;

public class followplayer : MonoBehaviour
{
   /* public Transform target;
    //public GameObject Atarget;
    Vector3 playerpos;
    public float smoothspeed = 0.125f;
    public Vector3 offset;
    public Vector3 HeightPos = new Vector3(0, 3, 11.6700001f);
   // public Slider zoomslider;
    //public Slider zoomInslider;

    Gamepadcontrols cont;
    


    // Use this for initialization

    private void Awake()
    {

        cont = new Gamepadcontrols();
        cont.Gameplay.zoomin.performed += ctx => ZoomIn();
        cont.Gameplay.Zoomout.performed += ctx => ZoomOut();
        
    }
    void Start()
    {
       // zoomslider = GameObject.FindGameObjectWithTag("ZoomSlider").GetComponent<Slider>();
        //zoomInslider = GameObject.FindGameObjectWithTag("ZoomInSlider").GetComponent<Slider>();

    }

    
    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null) { 

            transform.position = target.position + offset;
        }
        // mouse camera controls
        if (offset.y > (5+target.transform.localScale.x))
        {
            
                if (offset.y >= (27+ target.transform.localScale.x)) {
                    offset.z = -8.7f;
                 }
                offset += new Vector3(0f, -Input.mouseScrollDelta.y, (Input.mouseScrollDelta.y / 5));
                offset = Vector3.ClampMagnitude(offset, 30);

           // offset += new Vector3(0f, (zoomslider.value/4), ((zoomslider.value/4) / 5));
            //offset += new Vector3(0f, (-zoomInslider.value / 4), ((-zoomInslider.value / 4) / 5));
           // offset = Vector3.ClampMagnitude(offset, 30);




        }
        else {

            offset.y = (6f + target.transform.localScale.x);

            offset.z = -5.37f;
        }

        

        


    }
    
    //gamepade camera controls
    void ZoomIn() {

        if (offset.y > 5)
        {

            if (offset.y >= 27)
            {
                offset.z = -8.7f;
            }
            offset -= new Vector3(0f, -1, (10f/5));
            offset = Vector3.ClampMagnitude(offset, 30);






        }
        else
        {

            offset.y = 6f;

            offset.z = -5.37f;
        }


       
         
    }

    void ZoomOut() {

        if (offset.y > 5)
        {

            if (offset.y >= 27)
            {
                offset.z = -8.7f;
            }
            offset += new Vector3(0f, -1, (10f / 5));
            offset = Vector3.ClampMagnitude(offset, 30);






        }
        else
        {

            offset.y = 6f;

            offset.z = -5.37f;
        }



    }
    void OnEnable()
    {
        cont.Gameplay.Enable();
    }
    void OnDisable()
    {
        cont.Gameplay.Disable();
    }
   */
}
