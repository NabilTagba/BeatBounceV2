using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundHandler : MonoBehaviour
{
    public GameObject p1, p2;
    public Vector3 p1StartLoc, p2StartLoc;
    public int p1Score, p2Score = 0;
    public GameObject ball;
    public TMP_Text ScoreText;
    public bool preventMultiScoreTimer = false;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private Text winnerName;
    float timer = 3;
    [SerializeField] int endGameScore;
    // Start is called before the first frame update
    void Start()
    {
        //Assigns the starting location of the players
        p1StartLoc = p1.transform.position;
        p2StartLoc = p2.transform.position;
        ResetGameState();
    }

    private void Update()
    {
        if (p1Score == endGameScore)
        {
            winScreen.SetActive(true);
            winnerName.text = PlayerPrefs.GetString("NameOfPlayerOne");
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SceneManager.LoadScene(0);
            }

        }
        else if (p2Score == endGameScore)
        {
            winScreen.SetActive(true);
            winnerName.text = PlayerPrefs.GetString("NameOfPlayerTwo");
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SceneManager.LoadScene(0);
            }
        }

    }

    public void UpdateScore(bool whichPlayer)
    {
        if(!preventMultiScoreTimer)
        {
            //Increases scores for each player
            if (whichPlayer)
            {
                p1Score++;
            }
            else
            {
                p2Score++;
            }

            //Changes the text
            ScoreText.text = p1Score + " - " + p2Score;

            //Checks if either score is 3 or more
            CheckForWin();

            //Resets everything back to starting values
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
        //Makes sure no player has ownership of the ball
        p1.GetComponent<BallInteractions>().hasBall = false;
        p2.GetComponent<BallInteractions>().hasBall = false;

        //Resets the position and velocty of the players as well as preventing movement
        p1.GetComponent<PlayerMovement>().StopPlayerMovement();
        p2.GetComponent<PlayerMovement>().StopPlayerMovement();
        p1.GetComponent<Rigidbody>().velocity = Vector3.zero;
        p2.GetComponent<Rigidbody>().velocity = Vector3.zero;
        p1.transform.position = p1StartLoc;
        p2.transform.position = p2StartLoc;

        //Resets the position and velocity of the ball
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
