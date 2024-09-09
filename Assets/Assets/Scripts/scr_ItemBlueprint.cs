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

    public int itemID;
    private string itemName;

    public float InteractRange = 100;

    public PlayerMovementAdvanced playerscript;
  

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(playerscript.hit.transform.gameObject.TryGetComponent(out IInteractable interactObj) && playerscript.hit.distance <= InteractRange)
            {
                interactObj.Interact();
                Debug.Log(interactObj);
            }
        }
  

    }


}
