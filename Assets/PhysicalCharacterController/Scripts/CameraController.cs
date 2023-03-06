using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public float maxVerticalAngle;
    public GameObject body;
    [SerializeField] private float vertSensitivityBuffer;
    GameObject camPosOnBody;
    
    private float _mouseVerticalValue;
    private float MouseVerticalValue
    {
        get => _mouseVerticalValue;
        set
        {
            if (value == 0) return;
            
            //float verticalAngle = _mouseVerticalValue + value;
            //verticalAngle = Mathf.Clamp(verticalAngle, -maxVerticalAngle, maxVerticalAngle);
            //_mouseVerticalValue = verticalAngle;
        }
    }

    public float sensitivity;
    bool camDoesNotHaveBody = true;


    PlayerControls controls;
    Gamepad gameControllerOne;
    [SerializeField] private int playerIndex;
    private void Awake()
    {
       
    }

    private void Start()
    {
        controls = new PlayerControls();
        gameControllerOne = Gamepad.all[playerIndex];
        

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

            
            

            Quaternion finalRotation = Quaternion.Euler(
                  90 * (-gameControllerOne.rightStick.y.ReadValue()/ vertSensitivityBuffer),
            0, 0);

            cameraTransform.localRotation = finalRotation;

            body.transform.rotation = Quaternion.Euler(
            0,
            body.transform.localRotation.eulerAngles.y + gameControllerOne.rightStick.x.ReadValue() * sensitivity,
            0);

            if (gameControllerOne.rightStickButton.wasReleasedThisFrame && Cursor.visible == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (gameControllerOne.rightStickButton.wasReleasedThisFrame && Cursor.visible == false)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }



        }
        
    }


}
