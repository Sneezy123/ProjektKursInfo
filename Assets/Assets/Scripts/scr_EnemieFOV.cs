using UnityEngine;

public class scr_EnemieFOV : MonoBehaviour
{
    [Header("Sichtfeld 1 (breit)")]
    public float wideViewRadius = 10f;
    [Range(0, 360)]
    public float wideViewAngle = 250f;

    [Header("Sichtfeld 2 (eng)")]
    public float narrowViewRadius = 30f;
    [Range(0, 360)]
    public float narrowViewAngle = 15f;

    public bool CanSeePlayer;
    public Transform player;

    void Start()
    {

    }

    void Update()
    {
        CheckIfPlayerInSight();
    }

    void CheckIfPlayerInSight()
    {
        CanSeePlayer = false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= wideViewRadius)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleBetweenEnemyAndPlayer < wideViewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, distanceToPlayer))
                {
                    CanSeePlayer = true;
                    return;
                }
            }
        }

        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= narrowViewRadius)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleBetweenEnemyAndPlayer < narrowViewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, distanceToPlayer))
                {
                    CanSeePlayer = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wideViewRadius);

        Vector3 wideViewAngleA = DirFromAngle(-wideViewAngle / 2, false);
        Vector3 wideViewAngleB = DirFromAngle(wideViewAngle / 2, false);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + wideViewAngleA * wideViewRadius);
        Gizmos.DrawLine(transform.position, transform.position + wideViewAngleB * wideViewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, narrowViewRadius);

        Vector3 narrowViewAngleA = DirFromAngle(-narrowViewAngle / 2, false);
        Vector3 narrowViewAngleB = DirFromAngle(narrowViewAngle / 2, false);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + narrowViewAngleA * narrowViewRadius);
        Gizmos.DrawLine(transform.position, transform.position + narrowViewAngleB * narrowViewRadius);

        if (CanSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
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
