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

    public PlayerMovementAdvanced playerScript;
  

    void Start()
    {
        playerScript = getPlayerscript.playerScript;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerScript.hit.transform != null)
        {
            Debug.Log(playerScript.hit.transform.name);
            
            if(playerScript.hit.transform.gameObject.TryGetComponent(out IInteractable interactObj) && playerScript.hit.distance <= InteractRange)
            {
                Debug.Log("Interact" + interactObj);
                interactObj.Interact();
            }
        }
  

    }


}
