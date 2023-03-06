﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public float maxVerticalAngle;
    public GameObject body;
    
    GameObject camPosOnBody;
    
    private float _mouseVerticalValue;
    private float MouseVerticalValue
    {
        get => _mouseVerticalValue;
        set
        {
            if (value == 0) return;
            
            float verticalAngle = _mouseVerticalValue + value;
            verticalAngle = Mathf.Clamp(verticalAngle, -maxVerticalAngle, maxVerticalAngle);
            _mouseVerticalValue = verticalAngle;
        }
    }

    public float sensitivity;
    bool camDoesNotHaveBody = true;

    private void Start()
    {
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
        if (body != null && camDoesNotHaveBody)
        {

           
            /*if(body.GetComponent<PlayerInput>() != null)
            {
                camPosOnBody = body.GetComponent<PlayerInput>().camPos;
            }*/
            if (body.GetComponent<PlayerMovement>() != null)
            {
                camPosOnBody = body.GetComponent<PlayerMovement>().camPos;
                body.GetComponent<BallInteractions>().AssignCam(GetComponent<Camera>());
            }
            
            transform.SetParent(camPosOnBody.transform);
            transform.position = camPosOnBody.transform.position;
            camDoesNotHaveBody = false;
        }

        if (body != null)
        {


            MouseVerticalValue = Input.GetAxis("Mouse Y");

            Quaternion finalRotation = Quaternion.Euler(
                -MouseVerticalValue * sensitivity,
            0, 0);

            cameraTransform.localRotation = finalRotation;

            body.transform.rotation = Quaternion.Euler(
            0,
            body.transform.localRotation.eulerAngles.y + Input.GetAxis("Mouse X") * sensitivity,
            0);

            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }



        }
        
    }


}
