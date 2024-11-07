using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class scr_ItemBlueprint : MonoBehaviour
{


    private string itemName;

    public float InteractRange = 100;
    public static bool canInteract = false;

    public scr_PlayerMovement playerScript;


    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) | Input.GetMouseButtonDown(0) && playerScript.hit.transform != null)
        {
            if (playerScript.hit.transform.gameObject.TryGetComponent(out IInteractable interactObj) && playerScript.hit.distance <= InteractRange)
            {
                // Debug.Log("Interact " + interactObj);
                interactObj.Interact();
            }
        }


    }


}
