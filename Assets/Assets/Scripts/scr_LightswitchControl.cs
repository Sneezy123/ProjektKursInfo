using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_LightswitchControl : MonoBehaviour, IInteractable
{
    public Light[] lights;
    public bool isOn = true;

    private Material lightSwitchMat;

    private void Start()
    {
        lightSwitchMat = GetComponent<Renderer>().material;
    }
    public void Interact()
    {
        // Turn the light on or off
        for (int lightIdx = 0; lightIdx < lights.Length; lightIdx++)
        {
            lights[lightIdx].enabled = !lights[lightIdx].enabled;
        }
            isOn = !isOn;
            if (isOn) lightSwitchMat.color = new Color(0f, 1f, 0f);
            else lightSwitchMat.color = new Color(1f, 0f, 0f);
    }
}
