using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    private itemPickupManager itemPickupManager;


    void Start()
    {
        itemPickupManager = GameObject.Find("FlashlightHolder").GetComponent<itemPickupManager>();
    }

    public void Interact()
    {
        itemPickupManager.PickupItem(this.gameObject);
    }

    // Flashlight can't be dropped so
    /* void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            itemPickupManager.DropItem(this.gameObject);
        }
    } */
}
