using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Loading_star : MonoBehaviour
{
    // Start is called before the first frame update

    public Text star;
    public float Max_wait_time = 10;
    public float multiple_of_deltatime = 2;
    float timer;
    


    // Update is called once per frame
    void Update()
    {
        timer += multiple_of_deltatime * Time.deltaTime;
        // making the star disapear
        if (timer >= Max_wait_time) {
            star.text = "";
            timer = 0;
        }

        // making the star apear
        if (timer >= (Max_wait_time/2)) {
            star.text = "*";
        }

    }
   
}
