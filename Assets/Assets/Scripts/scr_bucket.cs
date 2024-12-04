using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_bucket : MonoBehaviour, IPickupable
{
    // Start is called before the first frame update

    [Header("Audio")]

    public AudioSource hitGround;
    public bool isFull;


    public bool Drop()
    {
        return true; // That means that the item will be dropped physically!
    }

    public void AfterDrop()
    {
        // do nothing
    }

    public bool Pickup()
    {
        return true;
    }

    public void AfterPickup()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(-81.575f, -107.379f, 113.948f));
        transform.localPosition = new Vector3(0.047f, -0.164f, 0.012f);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("WhatIsPlayer")) hitGround.Play();
    }
}
