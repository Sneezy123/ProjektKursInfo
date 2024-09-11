using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor;
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
        if (Input.GetKeyDown(KeyCode.E) && playerscript.hit.transform != null)
        {
            Debug.Log(playerscript.hit.transform.name);
            
            if(playerscript.hit.transform.gameObject.TryGetComponent(out IInteractable interactObj) && playerscript.hit.distance <= InteractRange)
            {
                Debug.Log("Interact" + interactObj);
                interactObj.Interact();
            }
        }
  

    }


}
