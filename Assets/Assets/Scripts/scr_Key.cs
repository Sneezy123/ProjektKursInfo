using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Key : MonoBehaviour, IPickupable
{

    public scr_PlayerMovement playerScript;


    // Start is called before the first frame update
    public int keyNum;

    [Header("Audio")]

    public AudioSource hitGround;

    
    public bool Drop()
    {
        playerScript.currentItem = 0;

        return true; // That means that the item will be dropped physically!
    }

    public bool Pickup()
    {
        playerScript.currentItem = keyNum;
        return true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("WhatIsPlayer")) hitGround.Play();
    }
}
