using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UtilMenu : MonoBehaviour
{

    public GameObject utilMenuUI;
    public GameObject[] tabs;
    public GameObject[] tabButtons;


    void Update()
    {
        if (Input.GetKeyDown(Keybinds.utilMenu))
        {
            if (pauseMenu.menuOpen && pauseMenu.gamePaused && !pauseMenu.pauseMenuOpen)
            {
                CloseMenu();
            }
            else if (!pauseMenu.menuOpen && !pauseMenu.gamePaused && !pauseMenu.pauseMenuOpen)
            {
                OpenMenu();
            }
        }   
    }

    public void OpenMenu()
    {
        utilMenuUI.SetActive(true);
        pauseMenu.menuOpen = true;
        pauseMenu.gamePaused = true;
    }

    public void CloseMenu()
    {
        utilMenuUI.SetActive(false);
        pauseMenu.menuOpen = false;
        pauseMenu.gamePaused = false;
    }

    public void OpenTab(int tabID)
    {
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }
        tabs[tabID].SetActive(true);

        foreach (GameObject tabButton in tabButtons)
        {
            tabButton.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }
        tabButtons[tabID].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
    }
}
