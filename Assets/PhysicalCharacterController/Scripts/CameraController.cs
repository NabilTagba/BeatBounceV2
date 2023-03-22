﻿using Photon.Pun.Demo.SlotRacer;
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
    public GameObject BallLocation;
    public GameObject OtherPlayer;

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

    float yInput = 0;
    PlayerControls controls;
    Gamepad gameControllerOne;
    [SerializeField] private int playerIndex;

    [SerializeField] GameObject pauseMenue;
    private void Awake()
    {
    }

    private void Start()
    {
        controls = new PlayerControls();
        if (Gamepad.all.Count > 1)
        {
            gameControllerOne = Gamepad.all[playerIndex];
        }
        else if (Gamepad.all.Count == 1 && playerIndex == 0)
        {
            gameControllerOne = Gamepad.all[0];
        }
        else
        {
            gameControllerOne = null;
        }

        if (PlayerPrefs.GetFloat("PlayerOneSensitivity") == 0 || PlayerPrefs.GetFloat("PlayerTwoSensitivity") == 0)
        {
            PlayerPrefs.SetFloat("PlayerOneSensitivity", .5f * 4);
            PlayerPrefs.SetFloat("PlayerTwoSensitivity", .5f * 4);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (playerIndex == 0)
        {
            sensitivity = PlayerPrefs.GetFloat("PlayerOneSensitivity");
        }
        else if (playerIndex == 1)
        {
            sensitivity = PlayerPrefs.GetFloat("PlayerTwoSensitivity");
        }

        if (gameControllerOne != null && gameControllerOne.startButton.wasPressedThisFrame)
        {
            if (pauseMenue.active == false)
            {
                pauseMenue.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pauseMenue.SetActive(false);
                Time.timeScale = 1;
            }
        }
        if (body != null && camDoesNotHaveBody && gameControllerOne != null)
        {
            

            if (body.GetComponent<PlayerInput>() != null)
            {
                camPosOnBody = body.GetComponent<PlayerInput>().camPos;
            }
            if (body.GetComponent<PlayerMovement>() != null)
            {
                camPosOnBody = body.GetComponent<PlayerMovement>().camPos;
                body.GetComponent<BallInteractions>().AssignCam(GetComponent<Camera>());
            }

            transform.SetParent(camPosOnBody.transform);
            transform.position = camPosOnBody.transform.position;
            camDoesNotHaveBody = false;
        }

        if (body != null && gameControllerOne != null)
        {


            yInput += -gameControllerOne.rightStick.y.ReadValue() * 200 * sensitivity * Time.deltaTime;

            Quaternion finalRotation = Quaternion.Euler(
             Mathf.Clamp(yInput, -90, 90),
             0, 0);

            //Look up and down
            cameraTransform.localRotation = finalRotation;

            //Look left and right
            body.transform.Rotate(0, gameControllerOne.rightStick.x.ReadValue() * sensitivity * 250 * Time.deltaTime, 0);


            //print("player "+ Gamepad.all[1] + " x value ="+ gameControllerOne.rightStick.x.ReadValue());
            //print("player " + Gamepad.all[0] + " x value =" + gameControllerOne.rightStick.x.ReadValue());

           /* if (gameControllerOne.rightStickButton.wasReleasedThisFrame && Cursor.visible == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (gameControllerOne.rightStickButton.wasReleasedThisFrame && Cursor.visible == false)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            */

        }

        if (gameControllerOne != null && gameControllerOne.leftTrigger.isPressed)
        {
            //Locks camera onto the ball
            if(!body.GetComponent<BallInteractions>().hasBall)
            {
                body.transform.LookAt(BallLocation.transform.position);
                body.transform.Rotate(0, -90, 0);
                //body.transform.rotation = Quaternion.Euler(body.transform.rotation.x, body.transform.rotation.y, 0);
                body.transform.localEulerAngles = new Vector3(body.transform.localEulerAngles.x, body.transform.localEulerAngles.y, 0);


            }
            //Locks camera onto the other player
            else
            {
                body.transform.LookAt(OtherPlayer.transform.position);
                body.transform.Rotate(0, -90, 0);
                body.transform.localEulerAngles = new Vector3(body.transform.localEulerAngles.x, body.transform.localEulerAngles.y, 0);
            }

            //body.transform.localRotation = Quaternion.Euler(0, body.transform.localRotation.y, body.transform.localRotation.z);
        }


    }

}