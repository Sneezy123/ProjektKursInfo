using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    public itemPickupManager itemPickupManager;
    public bool isHolding = false;


    void Start()
    {
        itemPickupManager = GameObject.Find("ItemHolder").GetComponent<itemPickupManager>();
    }

    public void Interact()
    {
        itemPickupManager.PickupItem(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            itemPickupManager.DropItem(this.gameObject);
        }
    }
}
