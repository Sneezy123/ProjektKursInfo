using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keypaddoorMechanics : MonoBehaviour, IInteractable
{
    private Animator tuerAnim;
    private bool tueroffen = false;

    public bool isLocked = true;

    [Header("Audio")]
    public AudioSource doorOpen;
    public AudioSource doorClose;
    public AudioSource doorLocked;
    
    private void Awake(){
    tuerAnim = gameObject.GetComponent<Animator>();

    }

    public void Interact()
    {
        



        if (!isLocked)
        {
            if (!tueroffen && tuerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !tuerAnim.IsInTransition(0))
            {

                tuerAnim.Play("tueranimation", 0, 0.0f);
                tueroffen = true;
                doorOpen.Play();
            }
            else if (tueroffen && tuerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !tuerAnim.IsInTransition(0))
            {
                tuerAnim.Play("tuerSchliessenAnimation", 0, 0.0f);
                tueroffen = false;
                doorClose.Play();
                isLocked = true;
            }
        }
        else
        {
            Debug.Log("doorisLocked");
            doorLocked.Play();
        }

    }
}
