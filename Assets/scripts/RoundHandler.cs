using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RoundHandler : MonoBehaviour
{
    public GameObject p1, p2;
    public Vector3 p1StartLoc, p2StartLoc;
    public int p1Score, p2Score = 0;
    public GameObject ball;
    public TMP_Text ScoreText;
    public bool preventMultiScoreTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        p1StartLoc = p1.transform.position;
        p2StartLoc = p2.transform.position;
        ResetGameState();
    }

    private void Update()
    {
        

    }

    public void UpdateScore(bool whichPlayer)
    {
        if(!preventMultiScoreTimer)
        {
            if (whichPlayer)
            {
                p1Score++;
            }
            else
            {
                p2Score++;
            }

            ScoreText.text = p1Score + " - " + p2Score;

            CheckForWin();

            ResetGameState();

            preventMultiScoreTimer = true;
            Invoke("ResetTimer", 2f);
        }
        
    }

    void ResetTimer()
    {
        preventMultiScoreTimer = false;
    }

    void ResetGameState()
    {
        p1.GetComponent<BallInteractions>().hasBall = false;
        p2.GetComponent<BallInteractions>().hasBall = false;

        p1.GetComponent<PlayerMovement>().StopPlayerMovement();
        p2.GetComponent<PlayerMovement>().StopPlayerMovement();
        p1.GetComponent<Rigidbody>().velocity = Vector3.zero;
        p2.GetComponent<Rigidbody>().velocity = Vector3.zero;
        p1.transform.position = p1StartLoc;
        p2.transform.position = p2StartLoc;

        ball.GetComponent<DodgeBallScript>().ResetBallPos();
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Collider>().enabled = true;

    }

    void CheckForWin()
    {
        if (p1Score == 3)
        {
            //Playe1 wins
        }
        else if (p2Score == 3)
        {
            //Player2 wins
        }
    }
}
