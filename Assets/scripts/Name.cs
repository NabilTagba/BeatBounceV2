using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Name : MonoBehaviour
{
    // Start is called before the first frame update
    public Text GamerTag;
    PhotonView view;
    void Start()
    {
        view = GetComponent<PhotonView>();

        if (view.IsMine) {
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
            GamerTag.text = PhotonNetwork.NickName;
        }
        if (!view.IsMine) {
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
            GamerTag.text = view.Owner.NickName;
        }    
        
        
        
    }

    
}
