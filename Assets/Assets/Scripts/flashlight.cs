using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    private itemPickupManager itemPickupManager;
    private Light lightCone;

    private bool hasHit = false;

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
        if (!itemPickupManager.isHolding) itemPickupManager.PickupItem(this.gameObject);
    }

    // Flashlight can't be dropped so
    void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.Q))
        {
            itemPickupManager.DropItem(this.gameObject);
        }*/
        
        if (Mathf.Abs(GetComponent<Rigidbody>().velocity.y) > 0)
        {
            hasHit = false;
        }
        if (GetComponent<Rigidbody>().velocity.y == 0)
        {
            hitGround.Play();
            hasHit = true;
        }

        if (Input.GetKeyDown(KeyCode.F) && itemPickupManager.isHolding)
        {
            lightCone.enabled =  !lightCone.enabled;

            if (lightCone.enabled) turnOn.Play();
            else if (!lightCone.enabled) turnOff.Play();
        }

    }
}
