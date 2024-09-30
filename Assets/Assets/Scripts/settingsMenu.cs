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

        sensDisplay = sensInputPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        sensSlider = sensInputPanel.transform.GetChild(0).GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (settingsMenuUI.activeSelf == true)
        {
            pauseMenu.menuOpen = true;
            pauseMenu.gamePaused = true;
            Time.timeScale = 0f;
        }
    }

    public void CloseMenu()
    {
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        pauseMenu.gamePaused = false;
        pauseMenu.menuOpen = false;
    }

    public void changeSensSlider(float newSens)
    {
        SettingsValues.mouseSensitivity = newSens * 10f;
    }



    public void updateSensDisplay()
    {
        sensDisplay.SetText((SettingsValues.mouseSensitivity / 10f).ToString());
    }

    public void updateSensSliderValue()
    {
        sensSlider.value = SettingsValues.mouseSensitivity / 10f;
    }
}
