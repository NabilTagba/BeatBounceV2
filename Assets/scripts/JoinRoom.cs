using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;


public class JoinRoom : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    string playername;
    public InputField NameIn;
    RoomOptions Roomoption;
    bool onjoinroomfail;
    private void Start()
    {
        onjoinroomfail = false;
        Roomoption = new RoomOptions();
        if (!PlayerPrefs.HasKey("PlayerName"))
        {
            return;
        }
        else {

            playername = PlayerPrefs.GetString("PlayerName");
            NameIn.text = playername;   
        }
    }

    public void joinRoom() {


        string playernickname = NameIn.text;
        PhotonNetwork.NickName = playernickname;
        PlayerPrefs.SetString("PlayerName", playernickname);
        
        Roomoption.MaxPlayers = 4;
        
        //PhotonNetwork.JoinRandomOrCreateRoom( );
        PhotonNetwork.JoinRandomRoom();
       
        
        
        
       
        
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        onjoinroomfail = true;
        PhotonNetwork.CreateRoom(Random.Range(1.0f, 10000000000000000000.0f).ToString(), Roomoption, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        
        

        PhotonNetwork.LoadLevel("GameSceneDemo");
       

    }
    public override void OnJoinedRoom()
    {
        
        
        if (onjoinroomfail == false) { 
           PhotonNetwork.LoadLevel("GameSceneDemo");
        }
       


    }

    public void charactters() {

       // PhotonNetwork.LoadLevel("Characters");
    }
}
