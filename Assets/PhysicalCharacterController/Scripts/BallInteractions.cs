using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInteractions : MonoBehaviour
{
    public GameObject ball;
    bool hasBall = false;
    bool chargingThrow = false;
    public float throwMultiplier = 1;
    float throwChargeTime = 5;
    float maxThrowMultiplier = 2;
    float throwForce = 30;
    bool catchActive = false;
    public KeyCode throwCatchKey = KeyCode.Mouse0;
    public PlayerMovement PM;
    public CapsuleCollider extendedCatchRange;
    public Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        //ball = GameObject.FindWithTag("Ball");
        ball = GameObject.Find("DodgeBall");
        PM = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(throwCatchKey))
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

        if(Input.GetKey(throwCatchKey) && chargingThrow)
        {
            if(throwMultiplier < maxThrowMultiplier)
            {
                throwMultiplier += Time.deltaTime / throwChargeTime;
            }
            else
            {
                throwMultiplier = maxThrowMultiplier;
            }
            
        }

        if(Input.GetKeyUp(throwCatchKey) && chargingThrow)
        {
            ThrowBall();
            chargingThrow = false;
        }

        if(hasBall)
        {
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            HoldingBallPosUpdate();
        }    
    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject db = collision.transform.parent.gameObject;
        if(db.tag == "BallTrigger")
        {
            if(db.gameObject.GetComponent<DodgeBallScript>().damageActive)
            {
                if(catchActive)
                {
                    CatchBall();
                }
                else
                {
                    //GET HIT BY BALL
                    Debug.Log("Player got hit by ball");
                    //NOT YET IMPLEMENTED
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
        ball.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * throwForce * throwMultiplier,ForceMode.Impulse);
        ball.GetComponent<Collider>().enabled = true;
        hasBall = false;
    }

    void StartCatch()
    {
        catchActive = true;
        extendedCatchRange.enabled = true;
        Invoke("EndCatch",.5f);
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
        ball.transform.position = transform.position;
    }

    public void AssignCam(Camera tempCamera)
    {
        playerCam = tempCamera;
    }
}
