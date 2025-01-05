using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteract : MonoBehaviour, IInteractable
{
    private Animator doorAnim;
    private bool isCarDoorOpen = false;
    private bool isTrunkDoorOpen = false;


    [Header("Audio")]

    public AudioSource carDoorOpen;
    public AudioSource carDoorClose;



    private void Start()
    {
        doorAnim = gameObject.GetComponent<Animator>();

    }

    public void Interact()
    {
        if (transform.name == "tuer_l")
        {

            Debug.Log("interact!");

            if (!isCarDoorOpen && doorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !doorAnim.IsInTransition(0))
            {

                doorAnim.Play("OpenCarDoor", 0, 0.0f);
                isCarDoorOpen = true;
                carDoorOpen.PlayDelayed(0f);
            }

            else if (isCarDoorOpen && doorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !doorAnim.IsInTransition(0))
            {
                doorAnim.Play("CloseCarDoor", 0, 0.0f);
                isCarDoorOpen = false;
                carDoorClose.PlayDelayed(0.85f);

            }
        }

        else if (transform.name == "kofferraum")
        {

            Debug.Log("interact!");

            if (!isTrunkDoorOpen && doorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !doorAnim.IsInTransition(0))
            {

                doorAnim.Play("OpenCarTrunk", 0, 0.0f);
                isTrunkDoorOpen = true;
                carDoorOpen.PlayDelayed(0f);
            }

            else if (isTrunkDoorOpen && doorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !doorAnim.IsInTransition(0))
            {
                doorAnim.Play("CloseCarTrunk", 0, 0.0f);
                isTrunkDoorOpen = false;
                carDoorClose.PlayDelayed(0.85f);
            }
        }
    }
}
