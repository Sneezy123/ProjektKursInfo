using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorMechanics : MonoBehaviour, IInteractable
{
    private Animator tuerAnim;
    private bool tueroffen = false;

    private bool isLocked = true;

    [Header("Audio")]
    public AudioSource doorOpen;
    public AudioSource doorClose;
    public AudioSource doorLocked;

    public PlayerMovementAdvanced playerscript;
    private void Start()
    {
        tuerAnim = gameObject.GetComponent<Animator>();
        playerscript = GameObject.Find("Player").GetComponent<PlayerMovementAdvanced>();
    }

    public void Interact()
    {
        if (playerscript.currentItem == 1)
        {
            isLocked = false;
        }



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
