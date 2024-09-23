using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class schlüssel_1 : MonoBehaviour, IInteractable
{

    public scr_PlayerMovement playerScript;
    // Start is called before the first frame update
    public int schlüsselnummer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Interact()
    {
        playerScript.currentItem = schlüsselnummer;

    }
}
