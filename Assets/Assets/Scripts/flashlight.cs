using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class flashlight : MonoBehaviour, IInteractable, IPickupable
{
    // Start is called before the first frame update
    private itemPickupManager itemPickupManager;
    private Light lightCone;

    public int flashlightTimer = 1000;

    [Header("Audio")]

    public AudioSource turnOn;
    public AudioSource turnOff;

    public AudioSource hitGround;

    void Start()
    {
        itemPickupManager = GameObject.Find("FlashlightHolder").GetComponent<itemPickupManager>();
        lightCone = transform.GetChild(0).GetComponent<Light>();
        lightCone.enabled = false;
    }

    public void Interact()
    {
        if (!itemPickupManager.isHolding) itemPickupManager.PickupItem(this.transform);
    }

    // Flashlight can't be dropped so

    void FixedUpdate()
    {
        if (lightCone.enabled)
        {
            if(flashlightTimer == 0)
            {
                lightCone.enabled = !lightCone.enabled;
                turnOff.Play();
            }else{
                flashlightTimer --;
            }
            

        }

    }
    void Update()
    {
        if (Input.GetKeyDown(Keybinds.drop) && Input.GetKey(KeyCode.LeftShift))
        {
            itemPickupManager.DropItem();
            lightCone.enabled = false;
        }

        if (Input.GetKeyDown(Keybinds.flashLightToggle) && itemPickupManager.isHolding)
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

    public bool Pickup() { return false; }
    public bool Drop() { return false; }
}
