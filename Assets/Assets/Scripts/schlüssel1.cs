using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class schlüssel_1 : MonoBehaviour, IInteractable, IItem
{

    public scr_PlayerMovement playerScript;
    private itemPickupManager itemPickupManager;


    // Start is called before the first frame update
    public int schlüsselnummer;

    [Header("Audio")]

    public AudioSource hitGround;

    void Start()
    {
        itemPickupManager = GameObject.Find("ItemHolder").GetComponent<itemPickupManager>();
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Q) && itemPickupManager.isHolding)
        {
            itemPickupManager.DropItem();
            playerScript.currentItem = 0;
        }
    }
    public void Interact()
    {
        if (!itemPickupManager.isHolding)
        {
            playerScript.currentItem = schlüsselnummer;
            itemPickupManager.PickupItem(this.transform);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WhatIsGround")) hitGround.Play();
    }
}
