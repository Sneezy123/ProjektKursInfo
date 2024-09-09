using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CameraMovement : MonoBehaviour
{
    [Header("Sensitivity Settings")]
    public float sensX;
    public float sensY;

    [Header("Shake Settings")]
    public float baseShakeIntensity = 0.1f;   // Grundintensität des Shakes im Stand
    public float maxShakeIntensity = 0.5f;    // Maximale Shake-Intensität bei Höchstgeschwindigkeit
    public float shakeSpeedMultiplier = 0.05f; // Wie stark der Speed den Shake beeinflusst
    public float shakeFrequency = 1f;         // Frequenz der Shake-Bewegungen

    public Transform orientation;
    public Transform camHolder;

    private Rigidbody playerRigidbody;
    private float xRotation;
    private float yRotation;

    private Vector3 originalCamPosition;

    private float shakeTimer = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Suche nach einem Rigidbody an der Kamera oder dem Spieler
        playerRigidbody = GetComponentInParent<Rigidbody>();

        // Speichere die ursprüngliche Position der Kamera für den Shake-Effekt
        originalCamPosition = camHolder.localPosition;
    }

    private void Update()
    {
        HandleCameraRotation();
        ApplyCameraShake();
    }

    private void HandleCameraRotation()
    {
        // Hol dir die Maus-Inputs
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Kamera und Orientierung rotieren
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void ApplyCameraShake()
    {
        // Finde die Geschwindigkeit des Spielers
        float playerSpeed = playerRigidbody != null ? playerRigidbody.velocity.magnitude : 0f;

        // Berechne die Shake-Intensität: Basisintensität + Geschwindigkeitseinfluss
        float shakeIntensity = Mathf.Lerp(baseShakeIntensity, maxShakeIntensity, playerSpeed * shakeSpeedMultiplier);

        // Verwende Perlin Noise für sanfteren, natürlicheren Shake
        shakeTimer += Time.deltaTime * shakeFrequency;
        Vector3 shakeOffset = new Vector3(
            Mathf.PerlinNoise(shakeTimer, 0) - 0.5f,
            Mathf.PerlinNoise(0, shakeTimer) - 0.5f,
            0
        ) * shakeIntensity;

        // Füge den Shake-Effekt zur Kamera hinzu
        camHolder.localPosition = originalCamPosition + shakeOffset;
    }

    private void OnDisable()
    {
        // Stelle sicher, dass die Kamera nach Deaktivierung ihre ursprüngliche Position wieder einnimmt
        camHolder.localPosition = originalCamPosition;
    }
}
