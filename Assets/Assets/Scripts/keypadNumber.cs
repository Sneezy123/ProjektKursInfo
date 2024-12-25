using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keypadNumber : MonoBehaviour, IInteractable
{
    public int number;
    private KeypadManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = transform.parent.GetComponent<KeypadManager>();
    }
    public void Interact()
    {
        manager.input(number);
        manager.colorNumbers(-1);
    }
}
