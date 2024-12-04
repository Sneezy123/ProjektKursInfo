using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runtimeManager : MonoBehaviour

{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioListener.pause = false;
    }

    void Update()
    {
        if (pauseMenu.gamePaused && pauseMenu.menuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            AudioListener.pause = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            AudioListener.pause = false;
        }
    }
}
