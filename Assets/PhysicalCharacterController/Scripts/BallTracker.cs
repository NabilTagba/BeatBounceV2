using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTracker : MonoBehaviour
{
    public GameObject ball;
    public GameObject groundTracker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y / 2, ball.transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, ball.transform.position.y/2, transform.localScale.z);

        groundTracker.transform.position = new Vector3(ball.transform.position.x, groundTracker.transform.position.y, ball.transform.position.z);
    }
}
