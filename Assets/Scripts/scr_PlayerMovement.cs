using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ERROR: Assets/Scripts/scr_PlayerMovement.cs(4,7): error CS0246: The type or namespace name 'TMPro' could not be found (are you missing a using directive or an assembly reference?)
using TMPro;
// ERROR: Assets/Scripts/scr_PlayerMovement.cs(5,19): error CS0234: The type or namespace name 'UI' does not exist in the namespace 'UnityEngine' (are you missing an assembly reference?)
using UnityEngine.UI;



public class PlayerMovementAdvanced : MonoBehaviour
{

    // Give them default values!

    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public float walkSpeed = 7f;
    public float sprintSpeed = 10f;
    public float vaultSpeed = 10f;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag = 0f;
//timwarhiertest
    [Header("Crouching")]
    public float crouchSpeed = 5f;
    public float crouchYScale = 0.7f;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight = 2;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("References")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        freeze,
        unlimited,
        walking,
        sprinting,
        vaulting,
        crouching,
        sliding
    }

    public bool crouching;
    public bool vaulting;

    public bool freeze;
    public bool unlimited;

    public bool restricted;



    public Camera Cam;
    public GameObject lastHit;

    private Collider[] hitColliders;
    private float playerRadius = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;

    }

    private void Update()
    {
        // ground check
        hitColliders = Physics.OverlapSphere(transform.position + (Vector3.up * (playerRadius / 2 - 0.02f * 2)), playerRadius * (1f - 0.02f) / 2, whatIsGround);
        grounded = 0 < hitColliders.Length;

        MyInput();
        SpeedControl();
        StateHandler();

        rb.drag = groundDrag;

        Ray positionFacing = Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(positionFacing, out hit))
        {
            lastHit = hit.transform.gameObject;

        }

    }



    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // start crouch
        if (Input.GetKeyDown(crouchKey) && horizontalInput == 0 && verticalInput == 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            crouching = true;
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

            crouching = false;
        }
    }

    bool keepMomentum;
    private void StateHandler()
    {
        // Mode - Freeze
        if (freeze)
        {
            state = MovementState.freeze;
            rb.velocity = Vector3.zero;
            desiredMoveSpeed = 0f;
        }

        // Mode - Unlimited
        else if (unlimited)
        {
            state = MovementState.unlimited;
            desiredMoveSpeed = 999f;
        }

        // Mode - Vaulting
        else if (vaulting)
        {
            state = MovementState.vaulting;
            desiredMoveSpeed = vaultSpeed;
        }

        // Mode - Crouching
        else if (crouching)
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }


        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;

        // deactivate keepMomentum
        if (Mathf.Abs(desiredMoveSpeed - moveSpeed) < 0.1f) keepMomentum = false;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        if (restricted) return;

        // calculate movement direction
        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;


        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 4f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.02f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;

    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }


    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
}
