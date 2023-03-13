using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Name : MonoBehaviour
{
    [SerializeField] int playerIndex;

    Text name;
    void Start()
    {
        name = GetComponent<Text>();
        if (playerIndex == 0)
        {
            name.text = PlayerPrefs.GetString("NameOfPlayerOne");
        }
        else if (playerIndex == 1)
        {
            name.text = PlayerPrefs.GetString("NameOfPlayerTwo");
        }
    }

    
}
