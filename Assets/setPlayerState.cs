using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setPlayerState : MonoBehaviour
{
    public scr_PlayerMovement playerMovement;
    private scr_PlayerMovement.MovementState savedState;

    void SetState(scr_PlayerMovement.MovementState desiredState = scr_PlayerMovement.MovementState.freeze)
    {
        savedState = playerMovement.GetMovementState();
        scr_PlayerMovement.canChangeState = false;
        playerMovement.SetMovementState(desiredState);
    }

    void ResetState()
    {
        playerMovement.SetMovementState(savedState);
    }

    public void FreezeState()
    {
        SetState(scr_PlayerMovement.MovementState.freeze);
        Debug.Log("freeze");
    }

    public void WalkState()
    {
        SetState(scr_PlayerMovement.MovementState.walking);
        Debug.Log("walk");
    }
}
