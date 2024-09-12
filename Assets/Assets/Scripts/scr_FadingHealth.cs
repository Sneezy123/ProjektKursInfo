// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class scr_FadingHealth : MonoBehaviour
// {
//     public float hitRange = 2f;
//     public float hitDelay = 1.5f;
//     public int damage = 1;

//     private float nextHitTime;
//     private bool canAttack = true;

//     void Start()
//     {
//         nextHitTime = Time.time;
//     }

//     void Update()
//     {
//         if (canAttack && playerHealth != null)
//         {
//             float distanceToPlayer = Vector3.Distance(transform.position, playerHealth.transform.position);

//             if (distanceToPlayer <= hitRange && Time.time >= nextHitTime)
//             {
//                 AttackPlayer();
//                 nextHitTime = Time.time + hitDelay + pauseTime;
//             }
//         }
//     }

//     void AttackPlayer()
//     {
//         playerHealth.TakeDamage(transform);
//         canAttack = false;
//         StartCoroutine(AttackPause());
//     }

//     IEnumerator AttackPause()
//     {
//         yield return new WaitForSeconds(pauseTime);
//         canAttack = true;
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             playerHealth = other.GetComponent<scr_PlayerHealth>();
//         }
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             playerHealth = null;
//         }
//     }

//     void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.white;

//         Gizmos.DrawWireSphere(transform.position, hitRange);
//     }
// }