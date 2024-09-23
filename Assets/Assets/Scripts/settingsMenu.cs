using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;

public class settingsMenu : MonoBehaviour
{
    public GameObject settingsMenuUI;
    public GameObject sceneManager;
    private SettingsValues settingsValues;
    public TextMeshProUGUI sensDisplay;
    void Start()
    {
        sensDisplay = settingsMenuUI.transform.Find("SensInputPanel/CurrentValueDisplay").GetComponent<TextMeshProUGUI>();
        SettingsValues settingsValues = sceneManager.GetComponent<SettingsValues>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSensValue();
    }

    public void CloseMenu()
    {
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        pauseMenu.gamePaused = false;
    }

    public void UpdateSensValue()
    {
        sensDisplay.text = settingsValues.mouseSensitivity.ToString();
    }
}
