using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovementAdvanced : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public float walkSpeed = 7f;
    public float sprintSpeed = 10f;
    public float vaultSpeed = 15f;
    public float groundDrag = 0f;

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
    private bool grounded;

    [Header("Slope Handling")]
    private bool onSlope;
    public float maxSlopeAngle = 50f;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Stamina")]
    public float maxStamina = 100f;
    private float currentStamina;
    public float staminaDrain = 20f; // Ausdauerverbrauch pro Sekunde
    public float staminaRegen = 10f; // Regenerationsrate pro Sekunde

    [Header("Post-Processing")]
    public float vignetteMinIntensity = 0.2f;
    public float vignetteMaxIntensity = 0.6f;
    public float vignettePulseSpeed = 6f;
    private Vignette vignetteEffect;
    private PostProcessVolume volume;
    private float t;
    private float t2;

    [Header("References")]
    public Transform orientation;
    public Camera cam;
    public scr_PostProcessingController pPController;

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
        crouching
    }

    public bool crouching;
    public bool vaulting;
    public bool freeze;
    public bool unlimited;
    public bool restricted;

    public Camera Cam;
    public RaycastHit hit;
    private Collider[] hitColliders;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = transform.localScale.y;

        pPController = GameObject.FindGameObjectWithTag("PostProcessing").GetComponent<scr_PostProcessingController>();

        currentStamina = maxStamina;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.1f, whatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();
        rb.drag = groundDrag;
        onSlope = OnSlope();

        // Regeneriere die Ausdauer, wenn nicht gesprintet wird
        if (state != MovementState.sprinting && currentStamina < maxStamina)
        {
            RegenerateStamina();
        }

        // Update Post-Processing basierend auf der Ausdauer
        UpdatePostProcessingEffects();
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
        currentStamina -= staminaDrain * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    private void RegenerateStamina()
    {
        currentStamina += staminaRegen * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    private void UpdatePostProcessingEffects()
    {
        if (vignetteEffect != null)
        {
            float staminaRatio = currentStamina / maxStamina;

            if (staminaRatio < 0.3f)
            {
                if(t != null) t2 = t;
                float pulse = Mathf.Sin(Time.time * vignettePulseSpeed) * (1 - staminaRatio);

                pPController.vignetteIntensity = pPController.vignetteIntensity + ((Mathf.Lerp(vignetteMaxIntensity, vignetteMinIntensity, staminaRatio) + pulse * 0.05f) - t2);
            }
            else
            {
                pPController.vignetteIntensity = t;
            }
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
