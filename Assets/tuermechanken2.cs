using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuermechanken2 : MonoBehaviour, IInteractable
{
    private Animator tuerAnim;
    private bool tueroffen = false;

    private void Awake(){
    tuerAnim = gameObject.GetComponent<Animator>();

    }

   public void Interact()
    {
                

        if (!tueroffen){

            tuerAnim.Play("tueranimation",0,0.0f);
            tueroffen= true;
        }
        else
        {
           tuerAnim.Play("tuerSchliessenAnimation",0,0.0f);
            tueroffen= false;

        }
    }
}
