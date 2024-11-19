using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_faucet : MonoBehaviour, IInteractable
{
    public scr_bucket bucket;
    public void Interact()
    {
        if (!bucket.isFull)
        {
            bucket.isFull = true;
        }

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
