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

    [Range(0, 1)] public float maxGrainIntensity = 0.5f;
    [Range(0, 1)] public float maxVignetteIntensity = 0.75f;
    [Range(0, 1)] public float maxMotionBlurIntensity = 0.85f;
    [Range(0, 1)] public float maxChromaticAberrationIntensity = 0.25f;

    private float minGrainIntensity;
    private float minVignetteIntensity;
    private float minMotionBlurIntensity;
    private float minChromaticAberrationIntensity;

    [Range(0, 2)] public float PostProcessingEffectsDistance = 0.5f;
    [Range(0, 2)] public float PostProcessingEffectsIntensety = 1;

    public Color startVignetterColor = new Color(90, 90, 90);
    public Color hurtColor1 = new Color(80, 0, 0);
    public Color hurtColor2 = new Color(95, 0, 0);
    public Color hurtColor3 = new Color(105, 0, 0);
    private Color vignetteColor;
    public float vignettePulseSpeed;

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
        vignetteColor = startVignetterColor;
    }

    void Update()
    {
        UpdatePostProcessingEffects();
    }

    void UpdatePostProcessingEffects()
    {
        // Reset initial values to minimum intensities
        grainIntensity = minGrainIntensity;
        vignetteIntensity = minVignetteIntensity;
        motionBlurIntensity = minMotionBlurIntensity;
        chromaticAberrationIntensity = minChromaticAberrationIntensity;

        // Update all contributing factors
        UpdateEnemieAIPostProcessingEffects();
        UpdateDmgHealthSystemPostProcessingEffects();
        UpdatePlayerControllerPostProcessingEffects();

        // Apply combined values with clamping to max limits
        vignette.intensity.value = Mathf.Clamp(vignetteIntensity, minVignetteIntensity, maxVignetteIntensity) * PostProcessingEffectsIntensety;
        vignette.color.value = vignetteColor;
        motionBlur.shutterAngle.value = Mathf.Clamp(motionBlurIntensity, minMotionBlurIntensity, maxMotionBlurIntensity)* PostProcessingEffectsIntensety;
        grain.intensity.value = Mathf.Clamp(grainIntensity, minGrainIntensity, maxGrainIntensity)* PostProcessingEffectsIntensety;
        chromaticAberration.intensity.value = Mathf.Clamp(chromaticAberrationIntensity, minChromaticAberrationIntensity, maxChromaticAberrationIntensity)* PostProcessingEffectsIntensety;
    }

    private void UpdateEnemieAIPostProcessingEffects()
    {
        // scr_EnemieAI
        float distanceToPlayer = Vector3.Distance(EnemieController.transform.position, EnemieController.player.position);
        float maxDistance = EnemieController.wideViewRadius;
        float t = Mathf.Clamp01(1 - (distanceToPlayer / maxDistance)) + PostProcessingEffectsDistance;

        // Combine enemy contributions
        grainIntensity += Mathf.Lerp(0.05f, 0.5f * PostProcessingEffectsIntensety, t);
        vignetteIntensity += Mathf.Lerp(0.05f, 0.2f * PostProcessingEffectsIntensety, t);
        motionBlurIntensity += Mathf.Lerp(0f, 100f * PostProcessingEffectsIntensety, t);
        chromaticAberrationIntensity += Mathf.Lerp(0.1f, 0.3f * PostProcessingEffectsIntensety, t);
    }

    private void UpdateDmgHealthSystemPostProcessingEffects()
    {
        // scr_DamageAndHealthSystem
        float t = Mathf.Clamp01(1);

        if (DmgHealthSystem.hurtLvl == 1)
        {
            vignetteColor = hurtColor1;
            vignetteIntensity += 0.1f * PostProcessingEffectsIntensety;
            chromaticAberrationIntensity += Mathf.Lerp(0.1f, 0.2f * PostProcessingEffectsIntensety, t);
        }
        else if (DmgHealthSystem.hurtLvl == 2)
        {
            vignetteColor = hurtColor2;
            vignetteIntensity += 0.15f * PostProcessingEffectsIntensety;
            chromaticAberrationIntensity += Mathf.Lerp(0.1f, 0.3f * PostProcessingEffectsIntensety, t);
        }
        else if (DmgHealthSystem.hurtLvl == 3)
        {
            vignetteColor = hurtColor3;
            vignetteIntensity += 0.15f * PostProcessingEffectsIntensety;
            chromaticAberrationIntensity += Mathf.Lerp(0.1f, 0.4f * PostProcessingEffectsIntensety, t);
        }
    }

    private void UpdatePlayerControllerPostProcessingEffects()
    {
        // scr_PlayerMovement

        float pulse = Mathf.Sin(Time.time * vignettePulseSpeed) * (1 - PlayerController.staminaRatio);
        vignetteIntensity += Mathf.Lerp(maxVignetteIntensity * 0.3f, minVignetteIntensity, PlayerController.staminaRatio) + pulse * 0.05f;
    }
}
