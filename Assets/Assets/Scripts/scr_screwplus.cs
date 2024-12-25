using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screwplus : MonoBehaviour, IInteractable
{
    public scr_PlayerMovement playerScript;
    public scr_doorblock block;
    private GameObject selectedGameObject;


    public void Interact()
    {
        if (scr_PlayerMovement.currentItem == 69420)
        {
            selectedGameObject = playerScript.hit.transform.gameObject;
            block.currentScrewCount--;
            Destroy(selectedGameObject);
        }
    }
}
