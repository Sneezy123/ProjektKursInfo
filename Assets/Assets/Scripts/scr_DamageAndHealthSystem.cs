using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class scr_DamageAndHealthSystem : MonoBehaviour
{
    [Header("Player Setup")]
    [Range (1, 10)] public int playerHealth = 3;
    private int hurtLvl = 0;
    private bool playerIsDead = false;


    [Header("Enemy Setup")]
    [Range (0, 10)] public float hitRange = 2f;
    [Range (0, 10)] public float hitDelay = 1.5f;
    [Range (1, 10)] public int damage = 1;
    private float nextHitTime;
    private bool canAttack = true;


    [Header("References")]
    public GameObject enemy;
    public GameObject player;
    public scr_PostProcessingController pPController;


    [Header("Hurt Indicators")]
    public Image image;
    public List<Sprite> IndicatorChoices;
    private int currentSprite = 0;
    public Color hurtColor1;
    public Color hurtColor2;
    public Color hurtColor3;


    [Header("Post-Processing")]
    [Range(0, 1)] public float PostProcessingEffectsIntensety = 1f;
    public PostProcessVolume Volume;
    private float vignetteIntensity;
    private float chromaticAberrationIntensity;
    private float tmp1;
    private float tmp2;


    void Start()
    {
        nextHitTime = Time.time;
        pPController = GameObject.FindGameObjectWithTag("PostProcessing").GetComponent<scr_PostProcessingController>();
    }

    void Update()
    {
        if (canAttack && playerHealth != 0 && !playerIsDead)
        {
            float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.transform.position);

            if (distanceToPlayer <= hitRange && Time.time >= nextHitTime)
            {
                hurtPlayer();
                nextHitTime = Time.time + hitDelay;
            }
        }

        UpdatePostProcessingEffects();
    }

    public void hurtPlayer()
    {
        if(hurtLvl < playerHealth)
        {
            currentSprite++;
            hurtLvl++;
            image.sprite = IndicatorChoices[currentSprite];
        } 
        else
        {
            print("Player Died");
            playerIsDead = true;
            player.GetComponent<PlayerMovementAdvanced>().freeze = true;
            enemy.GetComponent<scr_EnemieAI>().canMove = false; 
        }

        canAttack = false;
        StartCoroutine(AttackPause());
    }

    void UpdatePostProcessingEffects()
{
    float t = Mathf.Clamp01(1);

    int activeEffects = 0;

    if (hurtLvl >= 1) activeEffects++;

    pPController.UpdateActiveEffectCount(activeEffects);

    if (hurtLvl == 1) 
    {
        pPController.vignetterColor = hurtColor1;
        tmp1 = Mathf.Lerp(0.05f, 0.2f * PostProcessingEffectsIntensety, t)
        pPController.vignetteIntensity = (pPController.vignetteIntensity + Mathf.Lerp(0.05f, 0.2f * PostProcessingEffectsIntensety, t)) / 2;
        pPController.chromaticAberrationIntensity = (pPController.chromaticAberrationIntensity + Mathf.Lerp(0.1f, 0.2f * PostProcessingEffectsIntensety, t)) / 2;
    }
    else if (hurtLvl == 2) 
    {
        pPController.vignetterColor = hurtColor2;
        pPController.vignetteIntensity = (pPController.vignetteIntensity + Mathf.Lerp(0.05f, 0.4f * PostProcessingEffectsIntensety, t)) / 2;
        pPController.chromaticAberrationIntensity = (pPController.chromaticAberrationIntensity + Mathf.Lerp(0.1f, 0.3f * PostProcessingEffectsIntensety, t)) / 2;
    }
    else if (hurtLvl == 3)
    {
        pPController.vignetterColor = hurtColor3;
        pPController.vignetteIntensity = (pPController.vignetteIntensity + Mathf.Lerp(0.05f, 0.6f * PostProcessingEffectsIntensety, t)) / 2;
        pPController.chromaticAberrationIntensity = (pPController.chromaticAberrationIntensity + Mathf.Lerp(0.1f, 0.5f * PostProcessingEffectsIntensety, t)) / 2;
    }
}


    IEnumerator AttackPause()
    {
        yield return new WaitForSeconds(hitDelay);
        canAttack = true;
    }

    #region Debug

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(enemy.transform.position, hitRange);
    }

    #endregion
}
