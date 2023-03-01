using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Spawnplayers : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject BluesPlayer;
    public GameObject RedPlayer;
    GameObject player;
    public Camera PlayerCamera;
    GameObject playerprefab2;
    public float min_X;
    public float max_X;
    public float min_y;
    public float max_y;
    public float min_z;
    public float max_z;
    string skin;
    CameraController cam;
    void Start ()
    {

        //skin = PlayerPrefs.GetString("PlayerSkin");
        Vector3 randomPosition = new Vector3(Random.Range(min_X,max_X), Random.Range(min_y, max_y), Random.Range(min_z, max_z));
        int skin = Random.Range(1, 4);

        if (skin == 1 || skin == 2)
        {
            player = PhotonNetwork.Instantiate(RedPlayer.name, randomPosition, Quaternion.identity);
        }
        else if(skin == 3 || skin == 4)
        {

            player = PhotonNetwork.Instantiate(BluesPlayer.name, randomPosition, Quaternion.identity);
        }
        
        /*if (skin == "BluePlayer")
        {
            player = PhotonNetwork.Instantiate(BluesPlayer.name, randomPosition, Quaternion.identity);
            //player = PhotonNetwork.Instantiate(Blueslime.name, new Vector3(379.700012f, 4.30000019f, 392.399994f), Quaternion.identity, 0);
        }
        else if (skin == "RedPlayer")
        {
             player = PhotonNetwork.Instantiate(RedPlayer.name, randomPosition, Quaternion.identity);
            //player = PhotonNetwork.Instantiate(RedSlime.name, new Vector3(379.700012f, 4.30000019f, 392.399994f), Quaternion.identity, 0);
        }*/


        Debug.Log("i have joined the game");
        GameObject camera = GameObject.FindWithTag("MainCamera");
        //GameObject nametag = GameObject.FindGameObjectWithTag("NameTag");
        //GameTagFollowplayer Nametagscript= nametag.GetComponent<GameTagFollowplayer>();
        //Nametagscript.target = player.transform;
        cam = camera.GetComponent<CameraController>();
        cam.body = player;

    }
   


}
