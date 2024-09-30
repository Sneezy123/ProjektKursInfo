using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class schlüssel_1 : MonoBehaviour, IInteractable
{

    public scr_PlayerMovement playerScript;
    private itemPickupManager itemPickupManager;

    private Collider itemCollider;

    // Start is called before the first frame update
    public int schlüsselnummer;

    [Header("Audio")]

    public AudioSource hitGround;

    void Start()
    {
        itemPickupManager = GameObject.Find("ItemHolder").GetComponent<itemPickupManager>();
        itemCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (itemCollider.CompareTag("ground")) hitGround.Play();


        if (Input.GetKeyDown(KeyCode.Q) && itemPickupManager.isHolding)
        {
            itemPickupManager.DropItem(this.transform);
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
}
