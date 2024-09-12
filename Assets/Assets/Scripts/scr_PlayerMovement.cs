using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
    //timwarhiertest
    [Header("Crouching")]
    public float crouchSpeed = 3.5f;
    public float crouchYScale = 0.7f;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight = 2;
    public LayerMask EnemyField;
    public LayerMask whatIsGround = 64; // LayerMask 64: WhatIsGround
    private bool grounded;

    [Header("Slope Handling")]

    private bool onSlope;
    public float maxSlopeAngle = 50;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Stamina")]
    public float maxStamina = 100f;
    private float currentStamina = 100f;
    public float staminaDrain = 20f; // Ausdauerverbrauch pro Sekunde
    public float staminaRegen = 10f; // Regenerationsrate pro Sekunde

    [Header("Post-Processing")]
    public PostProcessVolume Volume;
    private Vignette vignetteEffect;

    public float vignetteMinIntensity = 0.2f; // Vignette, wenn die Ausdauer voll ist
    public float vignetteMaxIntensity = 0.6f; // Vignette, wenn die Ausdauer leer ist
    public float vignettePulseSpeed = 6f; // Pulsfrequenz

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
    private float playerRadius = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;

        Volume = GameObject.FindGameObjectWithTag("PostProcessing").GetComponent<PostProcessVolume>();


        if (Volume != null)
        {
            Volume.profile.TryGetSettings(out vignetteEffect);
        }
    }

    private void Update()
    {
        hitColliders = Physics.OverlapSphere(transform.position + (Vector3.up * (playerRadius / 2 - 0.02f * 2)), playerRadius * (1f - 0.02f) / 2, whatIsGround | EnemyField);
        grounded = 0 < hitColliders.Length;

        MyInput();
        SpeedControl();
        StateHandler();

        rb.drag = groundDrag;

        Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit);
        Debug.DrawRay(Cam.transform.position, Cam.transform.forward * 99f);

        onSlope = OnSlope();

        // Regeneriere die Ausdauer, wenn nicht gesprintet wird
        if (state != MovementState.sprinting && currentStamina < maxStamina)
        {
            RegenerateStamina();
        }

        // Update die Post-Processing-Effekte basierend auf der Ausdauer
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

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        if (desiredMoveSpeedHasChanged)
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
            Vector3 flatVel = new Vector3(rb.velocity.x, 4f, rb.velocity.z);

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
            // Pulsierende Vignette basierend auf der Ausdauer
            float staminaRatio = currentStamina / maxStamina;

            // Die Intensität wird stärker, je weniger Ausdauer man hat, und sie pulsiert mit einer Sinuskurve
            float pulse = Mathf.Sin(Time.time * vignettePulseSpeed) * (1 - staminaRatio);
            float vignetteIntensity = Mathf.Lerp(vignetteMaxIntensity, vignetteMinIntensity, staminaRatio) + pulse * 0.1f;

            vignetteEffect.intensity.Override(vignetteIntensity);
        }
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position + Vector3.up * playerHeight / 2, Vector3.down, out slopeHit))
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