using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_screwdriverplus : MonoBehaviour, IPickupable
{
   [Header("Audio")]

    public AudioSource hitGround;

    public bool Drop()
    {
        scr_PlayerMovement.currentItem = 0;
        return true; // That means that the item will be dropped physically!
    }

    public bool Pickup()
    {
        
        return true;
    }

    public void AfterDrop()
    {
        // do nothing
    }

    public void AfterPickup()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(-81.575f, -107.379f, 113.948f));
        transform.localPosition = new Vector3(0.047f, -0.164f, 0.012f);
        scr_PlayerMovement.currentItem = 69420;
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("WhatIsPlayer")) hitGround.Play();
    }
}

