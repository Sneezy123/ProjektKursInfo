using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class scr_PostProcessingController : MonoBehaviour
{
    [Header("Post-Processing")]
    public PostProcessVolume Volume;
    [Range(0, 1)] public float grainIntensity = 0.02f;
    [Range(0, 1)] public float vignetteIntensity = 0.02f;
    [Range(0, 1)] public float motionBlurIntensity = 0.5f;
    [Range(0, 1)] public float chromaticAberrationIntensity = 0.02f;

    [Range(0, 2)] public float PostProcessingEffectsDistance = 0.5f;
    [Range(0, 2)] public float PostProcessingEffectsIntensety = 1;

    [Range(0, 1)] public float maxGrainIntensity = 0.5f;
    [Range(0, 1)] public float maxVignetteIntensity = 0.75f;
    [Range(0, 1)] public float maxMotionBlurIntensity = 0.85f;
    [Range(0, 1)] public float maxChromaticAberrationIntensity = 0.25f;

    private float minGrainIntensity;
    private float minVignetteIntensity;
    private float minMotionBlurIntensity;
    private float minChromaticAberrationIntensity;

    private float DmgHealthSystemContribute;
    private float playerControllerContribute;
    private float EnemieContribute;

    public Color vignetterColor;
    
    public float pulseSpeed = 0f;
    private int activeEffectCount = 0; 

    private Grain grain;
    private Vignette vignette;
    private MotionBlur motionBlur;
    private ChromaticAberration chromaticAberration;


    [Header("Script Reference")]
    public scr_DamageAndHealthSystem DmgHealthSystem;
    public PlayerMovementAdvanced PlayerController;
    public scr_EnemieAI EnemieController;

    void Start()
    {
        Volume = GetComponent<PostProcessVolume>();

        if (Volume != null)
        {
            Volume.profile.TryGetSettings(out vignette);
            Volume.profile.TryGetSettings(out chromaticAberration);
            Volume.profile.TryGetSettings(out grain);
            Volume.profile.TryGetSettings(out motionBlur);
        }

        minGrainIntensity = grainIntensity;
        minVignetteIntensity = vignetteIntensity;
        minMotionBlurIntensity = motionBlurIntensity;
        minChromaticAberrationIntensity = chromaticAberrationIntensity;
    }

    void Update()
    {
        UpdatePostProcessingEffects();
    }

    public void UpdateActiveEffectCount(int effectCount)
    {
        activeEffectCount = effectCount;
        UpdatePulseSpeed();
    }

    private void UpdatePulseSpeed()
    {
        pulseSpeed = 1.0f + (0.5f * activeEffectCount);
    }

    void UpdatePostProcessingEffects()
    {
        // Scripts affecting PP:
        // scr_DamageAndHealthSystem
        // scr_PlayerMovement
        // scr_EnemieAI

        float t = Mathf.Clamp01(1);

        float staminaRatio = PlayerController.currentStamina / PlayerController.maxStamina;

        float distanceToPlayer = Vector3.Distance(EnemieController.transform.position, EnemieController.player.position);
        float maxDistance = EnemieController.wideViewRadius;
        float tEnemie = Mathf.Clamp01(1 - (distanceToPlayer / maxDistance)) + EnemieController.PostProcessingEffectsDistance;

        if (grain != null) 
        {
            // Initialisiere die Vignette-Intensität für diesen Frame
            playerControllerContribute = 0f;
            DmgHealthSystemContribute = 0f;
            EnemieContribute = 0f;


            // scr_EnemieAI
            grain.intensity.value = Mathf.Lerp(0.05f, 1f * PostProcessingEffectsIntensety, t);
        }

        if (vignette != null)
        {
            // Initialisiere die Vignette-Intensität für diesen Frame
            playerControllerContribute = 0f;
            DmgHealthSystemContribute = 0f;
            EnemieContribute = 0f;
            float totalVignetteIntensity = 0f;
            
            // scr_PlayerMovement
            if (staminaRatio < 0.3f)
            {
                float pulse = Mathf.Sin(Time.time * PlayerController.vignettePulseSpeed) * (1 - staminaRatio);
                playerControllerContribute = Mathf.Lerp(minVignetteIntensity, maxVignetteIntensity, staminaRatio) + pulse * 0.05f;
            }
            else
            {
                playerControllerContribute = 0f; 
            }

            // scr_DamageAndHealthSystem
            if (DmgHealthSystem.hurtLvl == 1) 
            {   
                vignetterColor = DmgHealthSystem.hurtColor1;
                DmgHealthSystemContribute = Mathf.Lerp(0.05f, 0.2f * PostProcessingEffectsIntensety, t);
            }
            else if (DmgHealthSystem.hurtLvl == 2) 
            {
                vignetterColor = DmgHealthSystem.hurtColor2;
                DmgHealthSystemContribute = Mathf.Lerp(0.05f, 0.4f * PostProcessingEffectsIntensety, t);
            }
            else if (DmgHealthSystem.hurtLvl == 3) 
            {
                vignetterColor = DmgHealthSystem.hurtColor3;
                DmgHealthSystemContribute = Mathf.Lerp(0.05f, 0.6f * PostProcessingEffectsIntensety, t);
            }
            else
            {
                DmgHealthSystemContribute = 0f;  // Keine Vignette vom Schaden, wenn hurtLvl = 0 ist
            }

            // scr_EnemieAI
            EnemieContribute = Mathf.Lerp(0.05f, 0.6f * PostProcessingEffectsIntensety, tEnemie);

            // Addiere die Beiträge von allen Quellen
            totalVignetteIntensity = DmgHealthSystemContribute + playerControllerContribute + EnemieContribute;

            totalVignetteIntensity = Mathf.Clamp(totalVignetteIntensity, minVignetteIntensity, maxVignetteIntensity);

            vignette.intensity.value = Mathf.PingPong(Time.time * pulseSpeed, totalVignetteIntensity);
        }


        if (motionBlur != null)
        {
            // Initialisiere die Vignette-Intensität für diesen Frame
            playerControllerContribute = 0f;
            DmgHealthSystemContribute = 0f;
            EnemieContribute = 0f;

            // scr_EnemieAI
            Mathf.Lerp(0f, 320f * PostProcessingEffectsIntensety, t);
        }  

        if (chromaticAberration != null) 
        {
            // Initialisiere die Vignette-Intensität für diesen Frame
            playerControllerContribute = 0f;
            DmgHealthSystemContribute = 0f;
            EnemieContribute = 0f;
            float totalChromaticAberrationIntensity = 0f;


            // scr_DamageAndHealthSystem
            if (DmgHealthSystem.hurtLvl == 1) 
            {   
                DmgHealthSystemContribute = Mathf.Lerp(0.1f, 0.2f * PostProcessingEffectsIntensety, t);
            }
            else if (DmgHealthSystem.hurtLvl == 2) 
            {
                DmgHealthSystemContribute = Mathf.Lerp(0.1f, 0.3f * PostProcessingEffectsIntensety, t);
            }
            else if (DmgHealthSystem.hurtLvl == 3) 
            {
                DmgHealthSystemContribute = Mathf.Lerp(0.1f, 0.5f * PostProcessingEffectsIntensety, t);
            }
            else
            {
                DmgHealthSystemContribute = 0f;  // Keine Vignette vom Schaden, wenn hurtLvl = 0 ist
            }

            // scr_EnemieAI
            EnemieContribute = Mathf.Lerp(0.1f, 0.85f * PostProcessingEffectsIntensety, t);

            totalChromaticAberrationIntensity = DmgHealthSystemContribute + playerControllerContribute + EnemieContribute;

            chromaticAberration.intensity.value = totalChromaticAberrationIntensity * PostProcessingEffectsIntensety;
        }
    }
}