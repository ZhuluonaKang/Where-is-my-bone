using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FlockingBehavior : MonoBehaviour
{
    public bool IsFlocking = true; 
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on this GameObject!");
        }
    }

    public void ApplyFlockingBehavior(Vector3 targetPosition)
    {
        if (IsFlocking && agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(targetPosition); 
        }
    }

    public void StopFlocking()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath(); 
        }
    }
}

