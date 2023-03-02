using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInteractions : MonoBehaviour
{
    public GameObject ball;
    bool hasBall = false;
    bool catchActive = false;
    public KeyCode throwCatchKey = KeyCode.Mouse0;

    // Start is called before the first frame update
    void Start()
    {
        //ball = GameObject.FindWithTag("Ball");
        ball = GameObject.Find("DodgeBall");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(throwCatchKey))
        {
            if (hasBall)
            {
                ThrowBall();
            }
            else
            {
                StartCatch();
            }
        }

        if(hasBall)
        {
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
        ball.GetComponent<Collider>().enabled = true;
        hasBall = false;
    }

    void StartCatch()
    {
        catchActive = true;
        Invoke("EndCatch",.5f);
    }

    void CatchBall()
    {
        ball.GetComponent<Collider>().enabled = false;
        hasBall = true;
    }

    void EndCatch()
    {
        catchActive = false;
    }

    void HoldingBallPosUpdate()
    {
        ball.transform.position = transform.position;
    }
}
