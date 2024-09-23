using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class schlüssel_1_test : MonoBehaviour, IInteractable
{

    private PlayerMovementAdvanced playerScript;
    // Start is called before the first frame update
    public int schlüsselnummer;
    void Start()
    {
        playerScript = PlayerMovementAdvanced.playerScript;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Interact()
    {
        playerScript.currentItem = schlüsselnummer;

    }
}
