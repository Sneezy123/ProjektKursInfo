using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class scr_Battery : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    public flashlight lights;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Interact()
    {
        lights.flashlightTimer = 1000;
        
    }
}
