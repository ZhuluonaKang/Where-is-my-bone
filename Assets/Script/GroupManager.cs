using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroupManager : MonoBehaviour
{
    public List<NavMeshAgent> slimes;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to the GroupManager!");
        }
    }

    public void Patrol(NavMeshAgent agent)
    {
        // Exit if there are no waypoints or the agent is null
        if (waypoints.Length == 0 || agent == null) return;

        // Set the agent's destination to the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        agent.SetDestination(targetWaypoint.position);

        // Move to the next waypoint if close enough to the current one
        if (Vector3.Distance(agent.transform.position, targetWaypoint.position) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    public void AddSlime(NavMeshAgent agent)
    {
        if (!slimes.Contains(agent))
        {
            slimes.Add(agent);
        }
    }

    public void RemoveSlime(NavMeshAgent agent)
    {
        if (slimes.Contains(agent))
        {
            slimes.Remove(agent);
        }
    }
}





