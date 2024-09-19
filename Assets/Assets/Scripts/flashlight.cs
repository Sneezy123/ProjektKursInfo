using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEditor.SearchService;
using UnityEngine;

public class flashlight : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update

    private PlayerMovementAdvanced playerscript;
    private GameObject itemHolder;
    public bool isHolding = false;


    void Start()
    {
        itemHolder = GameObject.Find("ItemHolder");
        playerscript = GameObject.Find("Player").GetComponent<PlayerMovementAdvanced>();

    }

    public void Interact()
    {
        transform.SetParent(itemHolder.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<MeshCollider>().enabled = false;

        isHolding = true;
    }
}
