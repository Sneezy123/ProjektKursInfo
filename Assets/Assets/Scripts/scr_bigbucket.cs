using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class scr_bigbucket : MonoBehaviour, IInteractable
{
    public scr_bucket bucket;
    public int barrelFullness;


    public void Interact()
    {
        if (bucket.isFull && barrelFullness <= 3 && scr_PlayerMovement.currentItem == 187)
        {
            barrelFullness++;
            bucket.isFull = false;
            if (barrelFullness == 4) runtimeManager.CompletePuzzle("barrel");
        }

        /* 
            Play animation for each new fill state (Like a cylinder that grows up)
         */
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
