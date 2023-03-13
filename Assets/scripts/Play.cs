using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Play : MonoBehaviour
{

    [SerializeField] InputField playerOneInputField;
    [SerializeField] InputField playerTwoInputField;

    /// <summary>
    /// starts the game
    /// </summary>
    public void PlayGame()
    {
        PlayerPrefs.SetString("NameOfPlayerOne", playerOneInputField.text);
        PlayerPrefs.SetString("NameOfPlayerTwo", playerTwoInputField.text);

        SceneManager.LoadScene(1);
    }
}
