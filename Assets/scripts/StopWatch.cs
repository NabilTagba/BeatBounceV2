using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StopWatch : MonoBehaviour
{
    // Start is called before the first frame update
    public float time;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 3.5) {
            SceneManager.LoadScene("loading");
        }

    }
}
