using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class schlüssel_1 : MonoBehaviour, IPickupable
{

    public scr_PlayerMovement playerScript;


    // Start is called before the first frame update
    public int schlüsselnummer;

    [Header("Audio")]

    public AudioSource hitGround;

    
    public bool Drop()
    {
        playerScript.currentItem = 0;

        return true; // That means that the item will be dropped physically!
    }

    public void Pickup()
    {
        playerScript.currentItem = schlüsselnummer;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WhatIsGround")) hitGround.Play();
    }
}
