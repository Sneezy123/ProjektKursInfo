using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_doorblock : MonoBehaviour, IInteractable
{
    public scr_PlayerMovement playerScript;
    public doorMechanics door;
    public int currentScrewCount = 4;
    private GameObject selectedGameObject;
    public bool showOutline = false;

    public void Update()
    {

        if (!showOutline && GetComponent<cakeslice.Outline>() != null)
        {
            GetComponent<cakeslice.Outline>().eraseRenderer = true;
        }
        else if (showOutline && GetComponent<cakeslice.Outline>() != null)
        {
            GetComponent<cakeslice.Outline>().eraseRenderer = false;
        }

        if (currentScrewCount == 0)
        {
            showOutline = true;
        }
    }

    public void Interact()
    {

        if (currentScrewCount == 0)
        {
            door.isLocked = false;
            Destroy(gameObject);
            door.showOutline = true;
        }
    }
}
