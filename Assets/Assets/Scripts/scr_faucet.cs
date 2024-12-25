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

        /* 
            Play animation in the bucket (Like a cylinder that grows up)
            Play sound of water filling the bucket
            Play faucet sound
            Play faucet animation
         */

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*  */
}
