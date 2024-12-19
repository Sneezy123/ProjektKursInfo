using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class scr_doorblock : MonoBehaviour, IInteractable
{
    public scr_PlayerMovement playerScript;
    public doorMechanics door;
    public int currentscrews = 4;
    private GameObject selectedGameObject;
    public void Interact()
    {
        if (currentscrews==0)
        {
            selectedGameObject = playerScript.hit.transform.gameObject;
            door.isLocked = false;
            Destroy(selectedGameObject);
        }
    }
}
