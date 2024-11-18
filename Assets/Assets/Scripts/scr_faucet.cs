using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_faucet : MonoBehaviour, IInteractable
{
    public scr_bucket bucket;
    public void Interact()
    {
        if (!bucket.isfull)
        {
            bucket.isfull = true;
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
