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
        ball = GameObject.FindWithTag("Ball");
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
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "BallTrigger")
        {
            Debug.Log("PickUpBall");
        }
    }

    void ThrowBall()
    {

    }

    void StartCatch()
    {

    }

    void CatchBall()
    {

    }

    void EndCatch()
    {

    }
}
