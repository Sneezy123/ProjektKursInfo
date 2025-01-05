using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorMechanics : MonoBehaviour, IInteractable
{
    private Animator tuerAnim;
    private bool tueroffen = false;

    public bool isLocked = true;
    public bool showOutline = true;


    [Header("Audio")]
    public AudioSource doorOpen;
    public AudioSource doorClose;
    public AudioSource doorLocked;

    public scr_PlayerMovement playerScript;
    
    private void Start()
    {
        tuerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!showOutline && GetComponent<cakeslice.Outline>() != null)
        {
            GetComponent<cakeslice.Outline>().eraseRenderer = true;
        }
        else if (showOutline && GetComponent<cakeslice.Outline>() != null)
        {
            GetComponent<cakeslice.Outline>().eraseRenderer = false;
        }
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
            }
        }
        else
        {
            Debug.Log("doorisLocked");
            doorLocked.Play();
        }

    }
}
