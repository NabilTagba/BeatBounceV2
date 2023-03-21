using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Slider playerOneSensitivitySlider;
    [SerializeField] Slider playerTwoSensitivitySlider;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("PlayerOneSensitivity", playerOneSensitivitySlider.value * 4);
        PlayerPrefs.SetFloat("PlayerTwoSensitivity", playerTwoSensitivitySlider.value * 4);
    }
}
