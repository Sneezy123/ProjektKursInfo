using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class pauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public static bool gamePaused = false;
    public static bool menuOpen = false;
    public static bool pauseMenuOpen = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Keybinds.pauseMenu))
        {
            if (gamePaused && menuOpen)
            {
                ResumeGame();
            }
            else if (!menuOpen)
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        
        gamePaused = true;
        menuOpen = true;
        pauseMenuOpen = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        
        gamePaused = false;
        menuOpen = false;
        pauseMenuOpen = false;
    }

    public void LoadSettingsMenu()
    {
        settingsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        menuOpen = true;
        pauseMenuOpen = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
