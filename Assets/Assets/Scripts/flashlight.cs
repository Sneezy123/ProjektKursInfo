using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    private itemPickupManager itemPickupManager;
    private Light lightCone;

    private Collider itemCollider;

    [Header("Audio")]

    public AudioSource turnOn;
    public AudioSource turnOff;

    public AudioSource hitGround;

    void Start()
    {
        itemPickupManager = GameObject.Find("FlashlightHolder").GetComponent<itemPickupManager>();
        lightCone = transform.GetChild(0).GetComponent<Light>();
        itemCollider = GetComponent<Collider>();
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

        if (itemCollider.CompareTag("ground")) hitGround.Play();

        if (Input.GetKeyDown(KeyCode.F) && itemPickupManager.isHolding)
        {
            lightCone.enabled =  !lightCone.enabled;

            if (lightCone.enabled) turnOn.Play();
            else if (!lightCone.enabled) turnOff.Play();
        }

    }
}
