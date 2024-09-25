using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class settingsMenu : MonoBehaviour
{
    public GameObject settingsMenuUI;
    private GameObject sensInputPanel;
    private TextMeshProUGUI sensDisplay;
    private Slider sensSlider;
    private TMP_InputField sensTextField;
    void Start()
    {
        sensInputPanel = settingsMenuUI.transform.Find("SensInputPanel").gameObject;

        sensDisplay = sensInputPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        sensSlider = sensInputPanel.transform.GetChild(1).GetComponent<Slider>();
        sensTextField = sensInputPanel.transform.GetChild(0).GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        // UpdateSensValue();
    }

    public void CloseMenu()
    {
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        pauseMenu.gamePaused = false;
    }

    public void changeSensSlider(float newSens)
    {
        SettingsValues.mouseSensitivity = newSens;
    }

    public void changeSensTextField(string newSens)
    {
        SettingsValues.mouseSensitivity = float.Parse(newSens);
    }



    public void updateSensDisplay()
    {
        sensDisplay.SetText(SettingsValues.mouseSensitivity.ToString());
    }

    public void updateSensSliderValue()
    {
        sensSlider.value = SettingsValues.mouseSensitivity;
    }

    public void updateSensTextFieldValue()
    {
        sensTextField.text = SettingsValues.mouseSensitivity.ToString();
    }
}
