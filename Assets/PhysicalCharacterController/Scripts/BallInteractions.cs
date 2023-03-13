using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallInteractions : MonoBehaviour
{
    public GameObject ball;
    public bool hasBall = false;
    bool chargingThrow = false;
    public float throwMultiplier = 1;
    float throwChargeTime = 5;
    float maxThrowMultiplier = 2;
    float throwForce = 30;
    public bool catchActive = false;
    public KeyCode throwCatchKey = KeyCode.Mouse0;
    public PlayerMovement PM;
    public CapsuleCollider extendedCatchRange;
    public Camera playerCam;
    public bool IsPlayer1;
    public GameObject RH;


    PlayerControls controls;
    Gamepad gameControllerOne;

    [SerializeField] GameObject ballHoldGO;
    [SerializeField] int playerIndex = 0;
    private void Awake()
    {
        controls = new PlayerControls();

    }

    // Start is called before the first frame update
    void Start()
    {
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

        //ball = GameObject.FindWithTag("Ball");
        ball = GameObject.Find("DodgeBall");
        PM = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (gameControllerOne != null && gameControllerOne.rightTrigger.wasPressedThisFrame)
        {
            if (hasBall)
            {
                throwMultiplier = 1;
                chargingThrow = true;
            }
            else
            {
                StartCatch();
            }
        }

        if (gameControllerOne != null && gameControllerOne.rightTrigger.isPressed && chargingThrow)
        {
            if (throwMultiplier < maxThrowMultiplier)
            {
                throwMultiplier += Time.deltaTime / throwChargeTime;
            }
            else
            {
                throwMultiplier = maxThrowMultiplier;
            }

        }

        if (gameControllerOne != null && gameControllerOne.rightTrigger.wasReleasedThisFrame && chargingThrow)
        {
            ThrowBall();
            chargingThrow = false;
        }

        if (hasBall)
        {
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            HoldingBallPosUpdate();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject db = collision.transform.parent.gameObject;
        if (db.tag == "BallTrigger" && !hasBall)
        {
            if (db.gameObject.GetComponent<DodgeBallScript>().damageActive)
            {
                if (catchActive)
                {
                    CatchBall();
                }
                else
                {
                    //GET HIT BY BALL
                    RH.GetComponent<RoundHandler>().UpdateScore(!IsPlayer1);
                }
            }
            else
            {
                CatchBall();
            }
        }
    }

    void ThrowBall()
    {
        ball.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * throwForce * throwMultiplier, ForceMode.Impulse);
        ball.GetComponent<Collider>().enabled = true;
        hasBall = false;
    }

    void StartCatch()
    {
        catchActive = true;
        extendedCatchRange.enabled = true;
        Invoke("EndCatch", .5f);
    }

    void CatchBall()
    {
        EndCatch();
        CancelInvoke("EndCatch");

        ball.GetComponent<Collider>().enabled = false;
        hasBall = true;
    }

    void EndCatch()
    {
        extendedCatchRange.enabled = false;
        catchActive = false;
    }

    void HoldingBallPosUpdate()
    {
        ball.transform.position = ballHoldGO.transform.position;
    }

    public void AssignCam(Camera tempCamera)
    {
        playerCam = tempCamera;
    }
}
