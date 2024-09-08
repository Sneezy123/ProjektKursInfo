using UnityEngine;
using System.Collections;

public class scr_EnemieFOV : MonoBehaviour
{
    [Header("Wide FOV")]
    public float wideViewRadius = 30f;   // Radius des weiten Sichtfelds
    [Range(0, 360)]
    public float wideViewAngle = 75f;   // Horizontaler Winkel des weiten Sichtfelds
    [Range(0, 180)]
    public float wideViewVerticalAngle = 25f; // Vertikaler Winkel des weiten Sichtfelds
    public float maxWideViewRadius = 50f; // Maximales Limit für das weite Sichtfeld

    [Header("Narrow FOV")]
    public float narrowViewRadius = 15f; // Radius des engen Sichtfelds
    [Range(0, 360)]
    public float narrowViewAngle = 270f;  // Horizontaler Winkel des engen Sichtfelds
    [Range(0, 180)]
    public float narrowViewVerticalAngle = 100f; // Vertikaler Winkel des engen Sichtfelds
    public float maxNarrowViewRadius = 25f; // Maximales Limit für das enge Sichtfeld
    public float maxNarrowViewAngle = 360f; // Maximales Limit für den Winkel des engen Sichtfelds

    [Header("FOV Settings")]
    public float viewHeightOffset = 0.37f; // Höhe, um den Ursprung des Sichtfelds (z.B. Augenhöhe) zu kontrollieren
    public float wideViewHeight = 1f;
    public float narrowViewHeight = 1f;
    public bool CanSeePlayer;
    public Transform player;

    [Header("Chase Settings")]
    public float chaseGrowthRate = 1f;  // Geschwindigkeit, mit der das Sichtfeld vergrößert wird
    public float resetFOVDelay = 5f;    // Verzögerung in Sekunden, bis das Sichtfeld zurückgesetzt wird
    private float originalWideViewRadius; // Zum Speichern des ursprünglichen weiten Sichtfeld-Radius
    private float originalNarrowViewRadius; // Zum Speichern des ursprünglichen engen Sichtfeld-Radius
    private float originalNarrowViewAngle;  // Zum Speichern des ursprünglichen engen Sichtfeld-Winkels
    public float sightRetentionTime = 3f;

    private bool isChasing = false;
    private Coroutine resetFOVCoroutine = null;

    private float sightRetentionTimer;
    private bool wasPlayerSeen;

    void Start()
    {
        sightRetentionTimer = 0f;
        wasPlayerSeen = false;

        originalWideViewRadius = wideViewRadius;
        originalNarrowViewRadius = narrowViewRadius;
        originalNarrowViewAngle = narrowViewAngle;
    }

    void Update()
    {
        CheckIfPlayerInSightNarrow();
        if (!CanSeePlayer)
        {
            CheckIfPlayerInSightWide();
        }


        if (wasPlayerSeen)
        {
            sightRetentionTimer -= Time.deltaTime;
            if (sightRetentionTimer <= 0)
            {
                CanSeePlayer = false;
                wasPlayerSeen = false;
            }
        }
    }

    void CheckIfPlayerInSightNarrow()
    {
        CanSeePlayer = false;

        // Berechne den Ursprung des Sichtfelds basierend auf der viewHeightOffset
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + viewHeightOffset, transform.position.z);

        // 1. Enges Sichtfeld prüfen
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
                    CanSeePlayer = true;
                    wasPlayerSeen = true;
                    sightRetentionTimer = sightRetentionTime; // Timer zurücksetzen
                    StartChasing(); // Spieler gesehen, Verfolgung starten
                }
            }
        }

        // Verfolgung stoppen, wenn der Spieler nicht mehr sichtbar ist und der Timer abgelaufen ist
        if (!CanSeePlayer && isChasing && !wasPlayerSeen)
        {
            StopChasing();
        }
    }

    void CheckIfPlayerInSightWide()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + viewHeightOffset, transform.position.z);

        float distanceToPlayer = Vector3.Distance(origin, player.position);


        // 2. Weites Sichtfeld prüfen, nur wenn nicht bereits im engen Sichtfeld gesehen
        if (!CanSeePlayer)
        {
            distanceToPlayer = Vector3.Distance(origin, player.position);
            if (distanceToPlayer <= wideViewRadius)
            {
                Vector3 directionToPlayer = (player.position - origin).normalized;
                float horizontalAngle = Vector3.Angle(transform.forward, directionToPlayer);
                float verticalAngle = Vector3.Angle(Vector3.ProjectOnPlane(directionToPlayer, Vector3.right), Vector3.ProjectOnPlane(transform.forward, Vector3.right));

                if (horizontalAngle < wideViewAngle / 2 && Mathf.Abs(verticalAngle) < wideViewVerticalAngle / 2)
                {
                    if (!Physics.Raycast(origin, directionToPlayer, out RaycastHit hit, distanceToPlayer))
                    {
                        CanSeePlayer = true;
                        wasPlayerSeen = true;
                        sightRetentionTimer = sightRetentionTime; // Timer zurücksetzen
                        StartChasing(); // Spieler gesehen, Verfolgung starten
                    }
                }
            }
        }

        // Verfolgung stoppen, wenn der Spieler nicht mehr sichtbar ist und der Timer abgelaufen ist
        if (!CanSeePlayer && isChasing && !wasPlayerSeen)
        {
            StopChasing();
        }
    }


    private void StartChasing()
    {
        if (!isChasing)
        {
            isChasing = true;

            // Falls ein Rücksetzvorgang läuft, abbrechen
            if (resetFOVCoroutine != null)
            {
                StopCoroutine(resetFOVCoroutine);
                resetFOVCoroutine = null;
            }
        }

        // Sichtfeld schrittweise vergrößern, aber nicht über das maximale Limit hinausgehen
        wideViewRadius += chaseGrowthRate * Time.deltaTime;
        wideViewRadius = Mathf.Min(wideViewRadius, maxWideViewRadius); // Begrenze das weite Sichtfeld

        narrowViewRadius += chaseGrowthRate * Time.deltaTime;
        narrowViewRadius = Mathf.Min(narrowViewRadius, maxNarrowViewRadius); // Begrenze das enge Sichtfeld

        // Zusätzlich das Sichtfeld im Winkel erweitern
        narrowViewAngle += chaseGrowthRate * Time.deltaTime * 20; // Eine Geschwindigkeit für den Winkel-Wachstum
        narrowViewAngle = Mathf.Min(narrowViewAngle, maxNarrowViewAngle); // Begrenze den Winkel des engen Sichtfelds
    }

    private void StopChasing()
    {
        if (isChasing)
        {
            isChasing = false;

            // Verzögerung starten, bevor das Sichtfeld zurückgesetzt wird
            resetFOVCoroutine = StartCoroutine(ResetViewRadiusAfterDelay());
        }
    }

    private IEnumerator ResetViewRadiusAfterDelay()
    {
        // Warte die angegebene Zeit (resetFOVDelay)
        yield return new WaitForSeconds(resetFOVDelay);

        // Setze die Sichtfelder auf die ursprünglichen Werte zurück
        wideViewRadius = originalWideViewRadius;
        narrowViewRadius = originalNarrowViewRadius;
        narrowViewAngle = originalNarrowViewAngle;
    }
    private void OnDrawGizmos()
    {
        // Berechne den Ursprung für die Gizmos-Darstellung (mit viewHeightOffset)
        Vector3 gizmoOrigin = new Vector3(transform.position.x, transform.position.y + viewHeightOffset, transform.position.z);

        // Zeichne das breite Sichtfeld als Zylinder
        Gizmos.color = Color.yellow;
        DrawCylinder(gizmoOrigin, wideViewRadius, wideViewAngle, wideViewHeight);

        // Zeichne das enge Sichtfeld als Zylinder
        Gizmos.color = Color.red;
        DrawCylinder(gizmoOrigin, narrowViewRadius, narrowViewAngle, narrowViewHeight);

        // Linie zum Spieler, wenn sichtbar
        if (CanSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(gizmoOrigin, player.position);
        }
    }

    // Methode zum Zeichnen eines Zylinders für das Sichtfeld
    private void DrawCylinder(Vector3 origin, float radius, float angle, float height)
    {
        int segments = 36; // Anzahl der Segmente für den Kreis
        float step = angle / segments;

        // Zeichne die Basis des Zylinders
        Vector3[] basePoints = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            float currentAngle = -angle / 2 + step * i;
            Vector3 dir = DirFromAngle(currentAngle, false); // Berechnet die Richtung entsprechend des Winkels
            basePoints[i] = origin + dir * radius;

            // Zeichne Linien vom Ursprung zur Basis
            Gizmos.DrawLine(origin, basePoints[i]);
        }

        // Zeichne den oberen Kreis des Zylinders (um Höhe des Zylinders versetzt)
        Vector3[] topPoints = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            topPoints[i] = basePoints[i] + Vector3.up * height;
            // Verbinde Basis und Oberseite
            Gizmos.DrawLine(basePoints[i], topPoints[i]);
        }

        // Zeichne den Kreis an der Basis und an der Oberseite des Zylinders
        for (int i = 0; i < segments; i++)
        {
            Gizmos.DrawLine(basePoints[i], basePoints[(i + 1) % segments]);
            Gizmos.DrawLine(topPoints[i], topPoints[(i + 1) % segments]);
        }
    }

    // Methode, um eine Richtung von einem Winkel zu berechnen
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
