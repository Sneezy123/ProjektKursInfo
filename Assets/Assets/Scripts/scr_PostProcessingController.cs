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

    [Range(0, 1)] public float PostProcessingEffectsDistance = 0.5f;
    [Range(0, 1)] public float PostProcessingEffectsIntensety = 1;

    public Color vignetterColor;
    
    public float pulseSpeed = 1.0f;
    private int activeEffectCount = 0; 

    private Grain grain;
    private Vignette vignette;
    private MotionBlur motionBlur;
    private ChromaticAberration chromaticAberration;

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
        if (grain != null) grain.intensity.value = grainIntensity * PostProcessingEffectsIntensety;
        if (vignette != null)
        {
            vignette.intensity.value = vignetteIntensity * PostProcessingEffectsIntensety;
            vignette.intensity.value += Mathf.PingPong(Time.time * pulseSpeed, vignetteIntensity);
        }
        if (motionBlur != null) motionBlur.shutterAngle.value = motionBlurIntensity * PostProcessingEffectsIntensety;
        if (chromaticAberration != null) chromaticAberration.intensity.value = chromaticAberrationIntensity * PostProcessingEffectsIntensety;
    }
}
