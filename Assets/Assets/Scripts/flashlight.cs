using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class flashlight : MonoBehaviour, IInteractable, IItem
{
    // Start is called before the first frame update
    private itemPickupManager itemPickupManager;
    private Light lightCone;

    [Header("Audio")]

    public AudioSource turnOn;
    public AudioSource turnOff;

    public AudioSource hitGround;

    void Start()
    {
        itemPickupManager = GameObject.Find("FlashlightHolder").GetComponent<itemPickupManager>();
        lightCone = transform.GetChild(0).GetComponent<Light>();
    }

    public void Interact()
    {
        if (!itemPickupManager.isHolding) itemPickupManager.PickupItem(this.transform);
    }

    // Flashlight can't be dropped so
    void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.Q))
        {
            itemPickupManager.DropItem(this.gameObject);
        }*/

        if (Input.GetKeyDown(KeyCode.F) && itemPickupManager.isHolding)
        {
            lightCone.enabled =  !lightCone.enabled;

            if (lightCone.enabled) turnOn.Play();
            else if (!lightCone.enabled) turnOff.Play();
        }

    }

    public void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WhatIsGround"))
        {
            Debug.Log("hit ground");
            hitGround.Play();
        }
    }
}
