using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;

public class PlayerMovementAdvanced : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public float walkSpeed = 7f;
    public float sprintSpeed = 10f;
    [HideInInspector] public float vaultSpeed = 15f;
    [HideInInspector] public float groundDrag = 0f;

    [Header("Crouching")]
    public float crouchSpeed = 3.5f;
    public float crouchYScale = 0.7f;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    public LayerMask enemyField;
    [HideInInspector] private bool grounded;

    [Header("Slope Handling")]
    [HideInInspector] private bool onSlope;
    public float maxSlopeAngle = 50f;
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

    [HideInInspector] float horizontalInput;
    [HideInInspector] float verticalInput;

    [HideInInspector] Vector3 moveDirection;
    [HideInInspector] Rigidbody rb;

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
    private Collider[] hitColliders;

    public int currentItem;
    public GameObject holdingItem;

    public static PlayerMovementAdvanced playerScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = transform.localScale.y;

        currentStamina = maxStamina;

        playerScript = GetComponent<PlayerMovementAdvanced>();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.1f, whatIsGround | enemyField);
        MyInput();
        SpeedControl();
        StateHandler();
        DropItem();
        rb.drag = groundDrag;
        onSlope = OnSlope();

        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit);
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 99f);

        // Regeneriere die Ausdauer, wenn nicht gesprintet wird
        if (state != MovementState.sprinting && currentStamina < maxStamina)
        {
            RegenerateStamina();
        }

        staminaRatio = currentStamina / maxStamina;

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
        if (Input.GetKeyDown(crouchKey) && horizontalInput == 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            crouching = true;
        }

        // Stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
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
        else if (crouching)
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (grounded && Input.GetKey(sprintKey) && currentStamina > 0)
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
            DrainStamina();
        }
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
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

        // Slope movement
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0) rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else if (grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
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

    private void DrainStamina()
    {
        if (state == MovementState.sprinting)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    private void RegenerateStamina()
    {
        if (state != MovementState.sprinting)
        {
            currentStamina += staminaRegen * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
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

    public void DropItem()
    {
        Transform itemHolder = cam.transform.parent.GetChild(2);

        if (itemHolder.childCount >= 1)
        {
            holdingItem = itemHolder.GetChild(0).gameObject;
        }
        if (Input.GetAxisRaw("Drop") == 1)
        {
            Debug.Log(holdingItem.GetComponent<MonoBehaviour>());
        }
    }
}
