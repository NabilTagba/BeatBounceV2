using System.Collections;
using System.Collections.Generic;
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
        PlayerPrefs.SetFloat("PlayerOneSensitivity", playerOneSensitivitySlider.value);
        PlayerPrefs.SetFloat("PlayerTwoSensitivity", playerTwoSensitivitySlider.value);
    }
}
