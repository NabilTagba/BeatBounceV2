using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBallScript : MonoBehaviour
{
    public bool damageActive = false;
    private float currentMoveSpeed;
    private float damagingSpeedMin = 7;
    private Rigidbody rb;
    public Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Updates speed variable
        CheckForSpeed();
        //Checks if it is fast enough to do damage
        CheckIfDamaging();
    }

    void CheckForSpeed()
    {
        currentMoveSpeed = Vector3.Magnitude(rb.velocity);
    }

    void CheckIfDamaging()
    {
        if (currentMoveSpeed >= damagingSpeedMin)
        {
            damageActive = true;
        }
        else
        {
            damageActive = false;
        }
    }

    public void ResetBallPos()
    {
        transform.position = startPos;
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        //Bouncy
        if (collision.gameObject.layer == 6)
        {
            
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 1.75f, rb.velocity.z);
        }
    }*/
}
