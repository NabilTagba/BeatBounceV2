using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTagFollowplayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    //public GameObject Atarget;
    Vector3 playerpos;
    public float smoothspeed = 0.125f;
    public Vector3 offset;
    public Vector3 HeightPos = new Vector3(0, 3, 11.6700001f);
    Vector3 startscale;
    void Start()
    {
        startscale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = startscale;
        transform.rotation = Quaternion.identity;
        
    }
}
