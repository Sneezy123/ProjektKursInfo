using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_bigbucket : MonoBehaviour, IInteractable
{
    public scr_bucket fullbucket;
    public int fullness;
    public void Interact()
    {
        if (fullbucket.isfull && fullness <= 4)
        {
            fullness++;
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
