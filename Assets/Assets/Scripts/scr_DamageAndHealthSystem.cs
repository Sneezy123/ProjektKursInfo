using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_DamageAndHealthSystem : MonoBehaviour
{
    [Header("Player Setup")]

    [Range (1, 10)] public int playerHealth = 3;
    

    #region Privates

    private int hurtLvl = 0;
    private bool playerIsDead = false;

    #endregion

    [Header("Enemy Setup")]

    [Range (0, 10)] public float hitRange = 2f;
    [Range (0, 10)] public float hitDelay = 1.5f;
    [Range (1, 10)] public int damage = 1;

    #region Privates

    private float nextHitTime;
    private bool canAttack = true;

    #endregion


    [Header("References")]

    public GameObject enemy;
    public GameObject player;


    [Header("Hurt Indicators")]

    public Image image;
    public List<Sprite> IndicatorChoices;

    private int currentSprite = 0;


    void Start()
    {
        nextHitTime = Time.time;
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
