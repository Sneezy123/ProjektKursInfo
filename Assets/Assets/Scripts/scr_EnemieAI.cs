using UnityEngine;
using UnityEngine.AI;

public class scr_EnemieAI : MonoBehaviour
{
    [Header("Wide FOV")]
    public float wideViewRadius = 30f;
    [Range(0, 360)]
    public float wideViewAngle = 75f;
    [Range(0, 180)]
    public float wideViewVerticalAngle = 25f;

    [Header("Narrow FOV")]
    public float narrowViewRadius = 15f;
    [Range(0, 360)]
    public float narrowViewAngle = 270f;
    [Range(0, 180)]
    public float narrowViewVerticalAngle = 100f;

    [Header("FOV Settings")]
    public float viewHeightOffset = 0.37f;
    public float sightRetentionTime = 3f;

    public Transform player;
    private NavMeshAgent agent;

<<<<<<< HEAD
=======
    private bool canSeePlayer;
    private float sightRetentionTimer;
    private bool isChasing;

>>>>>>> 78866e62 (Finished AI Tracking)
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sightRetentionTimer = 0f;
        isChasing = false;
    }

    void Update()
    {
<<<<<<< HEAD
        agent.destination = player.position;
=======
        CheckIfPlayerInSight();

        if (canSeePlayer)
        {
            agent.destination = player.position;
            sightRetentionTimer = sightRetentionTime;
            isChasing = true;
        }
        else if (sightRetentionTimer > 0)
        {
            sightRetentionTimer -= Time.deltaTime;
            agent.destination = player.position;
        }
        else if (!isChasing && !canSeePlayer && sightRetentionTimer == 0)
        {
            //Patrollieren
            isChasing = false;
            agent.destination = transform.position;
        }
>>>>>>> 78866e62 (Finished AI Tracking)
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

    private void OnDrawGizmos()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + viewHeightOffset, transform.position.z);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, wideViewRadius);

        Vector3 wideViewAngleA = DirFromAngle(-wideViewAngle / 2, false);
        Vector3 wideViewAngleB = DirFromAngle(wideViewAngle / 2, false);

        Gizmos.DrawLine(origin, origin + wideViewAngleA * wideViewRadius);
        Gizmos.DrawLine(origin, origin + wideViewAngleB * wideViewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, narrowViewRadius);

        Vector3 narrowViewAngleA = DirFromAngle(-narrowViewAngle / 2, false);
        Vector3 narrowViewAngleB = DirFromAngle(narrowViewAngle / 2, false);

        Gizmos.DrawLine(origin, origin + narrowViewAngleA * narrowViewRadius);
        Gizmos.DrawLine(origin, origin + narrowViewAngleB * narrowViewRadius);

        if (canSeePlayer | isChasing && sightRetentionTimer > 0)
        {
            Gizmos.color = Color.black;
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
