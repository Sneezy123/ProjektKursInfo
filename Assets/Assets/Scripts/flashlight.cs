using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update

    private PlayerMovementAdvanced playerScript;
    private GameObject flashlightHolder;
    public bool isHolding = false;


    void Start()
    {
        flashlightHolder = GameObject.Find("FlashlightHolder");
        playerScript = getPlayerscript.playerScript;
    }

    public void Interact()
    {
        transform.SetParent(flashlightHolder.transform);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<MeshCollider>().enabled = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        isHolding = true;
    }
}
