using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsMenu : MonoBehaviour
{
    public GameObject settingsMenuUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.gamePaused)
        {
            CloseMenu();
        }*/
    }

    public void CloseMenu()
    {
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        pauseMenu.gamePaused = false;
    }
}
