using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;

public class scr_PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    [HideInInspector] public float vaultSpeed = 15f;
    [HideInInspector] public float groundDrag = 0f;

    [Header("Crouching")]
    public float crouchYScale = 0.7f;
    public bool getWiderWhenCrouching = false;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask enemyField;
    /* [HideInInspector] */ public bool grounded;

    [Header("Slope Handling")]
    [HideInInspector] private bool onSlope;
    public float maxSlopeAngle = 60f;
    [HideInInspector] private RaycastHit slopeHit;
    [HideInInspector] private bool exitingSlope;

    [Header("Stamina")]
    [HideInInspector] public float maxStamina = 100f;
    [HideInInspector] public float currentStamina;
    public float staminaDrain = 20f;
    public float staminaRegen = 10f;
    [HideInInspector] public float staminaRatio;

    [Header("References")]
    public Transform orientation;
    public Camera cam;
    public CapsuleCollider playerCollider;

    [HideInInspector] float horizontalInput;
    [HideInInspector] float verticalInput;

    [HideInInspector] Vector3 moveDirection;
    [HideInInspector] public Vector3 localMoveDirection;
    [HideInInspector] public Rigidbody rb;


    public MovementState state;
    public enum MovementState
    {
        freeze,
        unlimited,
        walking,
        sprinting,
        vaulting,
        crouching
    }

    [HideInInspector] public bool crouching;
    [HideInInspector] public bool vaulting;
    [HideInInspector] public bool freeze;
    [HideInInspector] public bool unlimited;
    [HideInInspector] public bool restricted;

    public RaycastHit hit;
    public GameObject hitObject;
    public UnityEngine.UI.Slider staminaSlider;


    private Collider[] hitColliders;

    public int currentItem;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        currentStamina = maxStamina;
        playerHeight = 1.75f;
    }

    private void Update()
    {
        // crouchSpeed = walkSpeed * 0.7f;


        grounded = Physics.Raycast(transform.position - Vector3.down * (playerHeight/2 - 0.3f), Vector3.down, out slopeHit, playerHeight / 2 - 0.1f, whatIsGround | enemyField);
        Debug.DrawRay(transform.position - Vector3.down * (playerHeight/2 - 0.3f), Vector3.down * (playerHeight / 2 - 0.1f));
        MyInput();
        SpeedControl();
        StateHandler();
        rb.drag = groundDrag;
        onSlope = OnSlope();

        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit);
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 99f);

        // Regeneriere die Ausdauer, wenn nicht gesprintet wird
        staminaRatio = currentStamina / maxStamina;
        staminaSlider.value = staminaRatio;
        if (rb.velocity.magnitude < 4) StartCoroutine(RegenerateStaminaAfterSeconds(3f));


        // Update Post-Processing basierend auf der Ausdauer
        //UpdatePostProcessingEffects();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Crouch
        if (Input.GetKeyDown(Keybinds.crouchKey) && horizontalInput == 0)
        {
            playerHeight *= crouchYScale;
            playerCollider.height = playerHeight;
            if (getWiderWhenCrouching) playerCollider.radius = 0.77f;
            playerCollider.center = new Vector3(0.01f, playerHeight * 0.5f, 0.06f);
            crouching = true;
        }

        // Stop crouch
        if (Input.GetKeyUp(Keybinds.crouchKey))
        {
            playerHeight /= crouchYScale;
            playerCollider.height = playerHeight;
            if(getWiderWhenCrouching) playerCollider.radius = 0.32f;
            playerCollider.center = new Vector3(0.01f, playerHeight * 0.5f, 0.06f);
            crouching = false;
        }
    }

    private void StateHandler()
    {
        if (freeze)
        {
            state = MovementState.freeze;
            rb.velocity = Vector3.zero;
            desiredMoveSpeed = 0f;
        }
        else if (unlimited)
        {
            state = MovementState.unlimited;
            desiredMoveSpeed = 999f;
        }
        else if (vaulting)
        {
            state = MovementState.vaulting;
            desiredMoveSpeed = vaultSpeed;
        }
        else if (grounded)
        {
            if (grounded && Input.GetKey(Keybinds.sprintKey) && currentStamina > 0)
            {
                state = MovementState.sprinting;
                desiredMoveSpeed = sprintSpeed;
                if (rb.velocity.magnitude > 0.01) DrainStamina();
            }
            else if (crouching)
            {
                state = MovementState.crouching;
                desiredMoveSpeed = walkSpeed * 0.6f;
            }
            else
            {
                state = MovementState.walking;
                desiredMoveSpeed = walkSpeed;
            }
        }

        if (desiredMoveSpeed != lastDesiredMoveSpeed)
        {
            moveSpeed = desiredMoveSpeed;
        }
        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        if (restricted) return;

        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;
        localMoveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Slope movement
        if (OnSlope() && !exitingSlope)
        {
            rb.velocity = GetSlopeMoveDirection(moveDirection) * moveSpeed;
            if (rb.velocity.y > 0) rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else if (grounded)
        {
            rb.velocity = moveDirection * moveSpeed;
        }
        else if (!grounded)
        {
            rb.velocity = moveDirection * moveSpeed;
            
        }
    }

    private void SpeedControl()
    {
        // Slope limit
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }


    // Stamina has to stay zero for drainedTime seconds
    private void DrainStamina()
    {
        currentStamina -= staminaDrain * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    private void RegenerateStamina()
    {
        currentStamina += staminaRegen * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    IEnumerator RegenerateStaminaAfterSeconds(float seconds)
    {
        if (state != MovementState.sprinting && currentStamina < maxStamina)
        {
            yield return new WaitForSeconds(seconds);
            RegenerateStamina();
        }
    }


    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.1f))
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

}
