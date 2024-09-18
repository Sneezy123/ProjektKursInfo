using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class schlüssel_1_test : MonoBehaviour, IInteractable
{

    public PlayerMovementAdvanced playerscript;
    // Start is called before the first frame update
    public int schlüsselnummer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Interact()
    {
        playerscript.currentItem = schlüsselnummer;
        
    }
}
