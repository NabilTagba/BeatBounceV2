using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBallScript : MonoBehaviour
{
    public bool damageActive = false;
    private float currentMoveSpeed;
    private float damagingSpeedMin = 7;
    private Rigidbody rb;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
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
}
