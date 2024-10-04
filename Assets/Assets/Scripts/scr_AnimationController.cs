using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_AnimationController : MonoBehaviour
{
    public Animator animator;  // Der Animator des Spielers
    public scr_PlayerMovement playMovement;

    public Rigidbody rb;

    private float speed;
    private float previusSpeed = 0;
    private float AnimationSpeed;

    void Update()
    {
        speed = Mathf.Round(rb.velocity.magnitude * 10f) / 10f;
        
        if(speed != previusSpeed)
        {
            animator.SetFloat("Speed", speed);
            Debug.Log(speed + " " + previusSpeed);
        }

        previusSpeed = speed;

        if (Input.GetKeyDown(playMovement.crouchKey)) animator.SetBool("crouching", true);
        if (Input.GetKeyUp(playMovement.crouchKey)) animator.SetBool("crouching", false);

        float AnimationSpeed = Mathf.Clamp(speed, 0.1f, 1f);

        animator.SetFloat("AnimationSpeed", AnimationSpeed);
    }
}
