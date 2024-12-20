using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTargetChecker : MonoBehaviour
{
    public Transform[] waypoints; 
    public float waypointTolerance = 0.5f; 
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"NavMeshAgent component is missing on {gameObject.name}. Please add a NavMeshAgent!");
        }

        if (waypoints.Length > 0)
        {
            SetNextWaypoint();
        }
    }

    void Update()
    {
        if (agent.hasPath)
        {
            Patrol();
            DrawPath(agent.path);
        }
        
    }

    void Patrol()
    {
        if (waypoints.Length == 0 || agent == null) return;

        if (agent.remainingDistance <= waypointTolerance && !agent.pathPending)
        {
            SetNextWaypoint();
        }
    }

    void SetNextWaypoint()
    {
        Transform nextWaypoint = waypoints[currentWaypointIndex];
        NavMeshHit hit;

        if (NavMesh.SamplePosition(nextWaypoint.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning($"Waypoint {currentWaypointIndex} is not on NavMesh!");
        }

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) return;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.green, 0.1f);
        }
    }
}


