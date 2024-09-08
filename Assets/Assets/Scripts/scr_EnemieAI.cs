using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class scr_EnemieAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    public scr_EnemieFOV EnemieFOV;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (EnemieFOV.CanSeePlayer)
        {
            agent.destination = player.position;
        }
    }
}
