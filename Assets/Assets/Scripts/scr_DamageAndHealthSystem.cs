using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scr_DamageAndHealthSystem : MonoBehaviour
{
    [Header("Player Setup")]
    [Range(1, 10)] public int playerHealth = 3;
    public int hurtLvl = 0;
    [HideInInspector] public bool playerIsDead = false;

    [Header("Enemy Setup")]
    [Range(0, 10)] public float hitRange = 2f;
    [Range(0, 10)] public float hitDelay = 1.5f;
    [Range(1, 10)] public int damage = 1;
    private float nextHitTime;
    private bool canAttack = true;

    [Header("References")]
    public List<GameObject> enemies = new List<GameObject>(); // List of enemies
    public GameObject player;

    public TextMeshProUGUI youHaveDiedText;

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
            foreach (GameObject enemy in enemies)
            {
                float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.transform.position);

                if (distanceToPlayer <= hitRange && Time.time >= nextHitTime)
                {
                    hurtPlayer();
                    nextHitTime = Time.time + hitDelay;
                }
            }
        }
    }

    public void hurtPlayer()
    {
        if (hurtLvl < playerHealth)
        {
            currentSprite++;
            hurtLvl++;
            image.sprite = IndicatorChoices[currentSprite];
        }
        else
        {
            Debug.Log("Player Died");
            playerIsDead = true;
            player.GetComponent<scr_PlayerMovement>().freeze = true;
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<scr_EnemieAI>().canMove = false;
            }
            youHaveDiedText.enabled = true;
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
        foreach (GameObject enemy in enemies)
        {
            Gizmos.DrawWireSphere(enemy.transform.position, hitRange);
        }
    }

    #endregion
}
