using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class scr_EnemieAI : MonoBehaviour
{
    [Header("Wide FOV")]
    public float wideViewRadius = 30f;
    [Range(0, 360)] public float wideViewAngle = 75f;
    [Range(0, 180)] public float wideViewVerticalAngle = 25f;


    [Header("Narrow FOV")]
    public float narrowViewRadius = 15;
    [Range(0, 360)] public float narrowViewAngle = 270f;
    [Range(0, 180)] public float narrowViewVerticalAngle = 100f;


    [Header("FOV Settings")]
    public float viewHeightOffset = 0.37f;
    public float sightRetentionTime = 1.5f;


    [Header("Patrolling Settings")]
    public float smallPatrolRadius = 5;
    public float smallPatrolDuration = 2f;
    public Transform[] waypoints;
    public float waypointPauseTime = 2f;
    public float narrowFOVAtWaypoint = 330f;

    [Header("Speed Settings")]
    public float chaseSpeed = 7f;
    public float patrolSpeed = 3f;

    [Header("Audio")]
    public AudioSource chaseAudio;

    [Header("Post-Processing")]
    private float grainIntensity;
    private float vignetteIntensity;
    private float motionBlurIntensity;
    private float chromaticAberrationIntensity;
    private float t;
    private float t2;

    [Range(0, 1)] public float PostProcessingEffectsDistance = 0.5f;
    [Range(0, 1)] public float PostProcessingEffectsIntensety = 1;


    [Header("References")]
    public scr_PostProcessingController pPController;
    public Transform player;
    private NavMeshAgent agent;

    public bool canSeePlayer = false;
    public bool isChasing = false;
    public bool isPatrolling = false;
    public bool isSearching = false;

    public bool canMove = true;

    private Vector3 lastKnownPlayerPosition;
    private int currentWaypointIndex = 0;
    private float smallPatrolTimer = 0f;
    private float waypointWaitTimer = 0f;
    private float originalNarrowViewAngle;
    private float sightRetentionTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sightRetentionTimer = 0f;

        isChasing = false;
        isSearching = false;
        canSeePlayer = false;

        originalNarrowViewAngle = narrowViewAngle;
        pPController = GameObject.FindGameObjectWithTag("PostProcessing").GetComponent<scr_PostProcessingController>();

        agent.speed = patrolSpeed;
        isChasing = false;
    }

    void Update()
    {
        if(!canMove)
        {
            agent.transform.position = transform.position;
        }
        else
        {
            CheckIfPlayerInSight();

            if (canSeePlayer)
            {
                agent.speed = chaseSpeed;
                agent.destination = player.position;
                sightRetentionTimer = sightRetentionTime;
                isChasing = true;
                isSearching = false;
                isPatrolling = false;
                ResetNarrowFOV();

                if (!chaseAudio.isPlaying)
                {
                    chaseAudio.Play();
                }
            }
            else if (sightRetentionTimer > 0)
            {
                agent.speed = chaseSpeed;
                sightRetentionTimer -= Time.deltaTime;
                agent.destination = player.position;
            }
            else if (isChasing && sightRetentionTimer <= 0 && !canSeePlayer)
            {
                // Der Gegner sucht nach dem Spieler
                if (!isSearching)
                {
                    lastKnownPlayerPosition = player.position;
                    agent.destination = lastKnownPlayerPosition;
                    isSearching = true;
                }

                if (chaseAudio.isPlaying && !isSearching)
                {
                    chaseAudio.Stop();
                }

                if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < 1f)
                {
                    PerformSmallPatrol();
                }
            }
            else if (!isChasing && !canSeePlayer && !isSearching && sightRetentionTimer <= 0)
            {
                // Der Gegner patrouilliert, wenn er den Spieler nicht sieht
                isPatrolling = true;
                PerformLargePatrol();
            }

            UpdatePostProcessingEffects();
        }
    }


    void CheckIfPlayerInSight()
    {
        canSeePlayer = false;

        Vector3 origin = new Vector3(transform.position.x, transform.position.y + viewHeightOffset, transform.position.z);
        float distanceToPlayer = Vector3.Distance(origin, player.position);

        if (distanceToPlayer <= narrowViewRadius)
        {
            Vector3 directionToPlayer = (player.position - origin).normalized;
            float horizontalAngle = Vector3.Angle(transform.forward, directionToPlayer);
            float verticalAngle = Vector3.Angle(Vector3.ProjectOnPlane(directionToPlayer, Vector3.right), Vector3.ProjectOnPlane(transform.forward, Vector3.right));

            if (horizontalAngle < narrowViewAngle / 2 && Mathf.Abs(verticalAngle) < narrowViewVerticalAngle / 2)
            {
                if (!Physics.Raycast(origin, directionToPlayer, out RaycastHit hit, distanceToPlayer))
                {
                    canSeePlayer = true;
                }
            }
        }

        if (!canSeePlayer && distanceToPlayer <= wideViewRadius)
        {
            Vector3 directionToPlayer = (player.position - origin).normalized;
            float horizontalAngle = Vector3.Angle(transform.forward, directionToPlayer);
            float verticalAngle = Vector3.Angle(Vector3.ProjectOnPlane(directionToPlayer, Vector3.right), Vector3.ProjectOnPlane(transform.forward, Vector3.right));

            if (horizontalAngle < wideViewAngle / 2 && Mathf.Abs(verticalAngle) < wideViewVerticalAngle / 2)
            {
                if (!Physics.Raycast(origin, directionToPlayer, out RaycastHit hit, distanceToPlayer))
                {
                    canSeePlayer = true;
                }
            }
        }
    }

    void PerformSmallPatrol()
    {
        agent.speed = chaseSpeed;

        if (smallPatrolTimer > 0)
        {
            smallPatrolTimer -= Time.deltaTime;

            if (!agent.hasPath || agent.remainingDistance < 0.2f)
            {
                Vector3 randomPoint = lastKnownPlayerPosition + Random.insideUnitSphere * smallPatrolRadius;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, smallPatrolRadius, NavMesh.AllAreas))
                {
                    agent.destination = hit.position;
                }
            }
        }
        else
        {
            isSearching = false;
            isChasing = false;
            canSeePlayer = false;
            isPatrolling = true;

            FindNearestWaypoint();

            if (waypointWaitTimer <= 0f)
            {
                agent.destination = waypoints[currentWaypointIndex].position;
                waypointWaitTimer = waypointPauseTime;
                ResetNarrowFOV();
            }
            else
            {
                waypointWaitTimer -= Time.deltaTime;
                narrowViewAngle = narrowFOVAtWaypoint;
            }            
        }
    }

    void PerformLargePatrol()
    {
        if (waypoints.Length == 0) return;

        agent.speed = patrolSpeed;

        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            if (waypointWaitTimer <= 0f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.destination = waypoints[currentWaypointIndex].position;
                waypointWaitTimer = waypointPauseTime;
                ResetNarrowFOV();
            }
            else
            {
                waypointWaitTimer -= Time.deltaTime;
                narrowViewAngle = narrowFOVAtWaypoint;
            }
        }
    }

    void FindNearestWaypoint()
    {
        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        currentWaypointIndex = closestIndex;
        agent.destination = waypoints[currentWaypointIndex].position;
    }

    void ResetNarrowFOV()
    {
        narrowViewAngle = originalNarrowViewAngle;
    }

    void UpdatePostProcessingEffects()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float maxDistance = wideViewRadius;

        if(t != null) t2 = t;
        t = Mathf.Clamp01(1 - (distanceToPlayer / maxDistance)) + PostProcessingEffectsDistance;

        int activeEffects = 0;

        if (distanceToPlayer < narrowViewRadius) activeEffects++;

        pPController.UpdateActiveEffectCount(activeEffects);

        if(t2 != null) 
        {
            pPController.grainIntensity = pPController.grainIntensity + (Mathf.Lerp(0.05f, 1f * PostProcessingEffectsIntensety, t) - t2);
            pPController.vignetteIntensity = pPController.vignetteIntensity + (Mathf.Lerp(0.05f, 0.6f * PostProcessingEffectsIntensety, t) - t2);
            pPController.motionBlurIntensity = pPController.motionBlurIntensity + (Mathf.Lerp(0f, 320f * PostProcessingEffectsIntensety, t) - t2);
            pPController.chromaticAberrationIntensity = pPController.chromaticAberrationIntensity + (Mathf.Lerp(0.1f, 0.85f * PostProcessingEffectsIntensety, t) - t2);
        }
    }



    private void OnDrawGizmosSelected()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + viewHeightOffset, transform.position.z);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, wideViewRadius);

        Vector3 wideViewAngleA = DirFromAngle(-wideViewAngle / 2, false);
        Vector3 wideViewAngleB = DirFromAngle(wideViewAngle / 2, false);

        Gizmos.DrawLine(origin, origin + wideViewAngleA * wideViewRadius);
        Gizmos.DrawLine(origin, origin + wideViewAngleB * wideViewRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, narrowViewRadius);

        Vector3 narrowViewAngleA = DirFromAngle(-narrowViewAngle / 2, false);
        Vector3 narrowViewAngleB = DirFromAngle(narrowViewAngle / 2, false);

        Gizmos.DrawLine(origin, origin + narrowViewAngleA * narrowViewRadius);
        Gizmos.DrawLine(origin, origin + narrowViewAngleB * narrowViewRadius);

        if (canSeePlayer | isChasing && sightRetentionTimer > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, player.position);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
