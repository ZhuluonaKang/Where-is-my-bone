using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager : MonoBehaviour
{
    public List<FlockingBehavior> slimes; 
    public Transform[] waypoints; 
    private int currentWaypointIndex = 0;

    void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to the GroupManager!");
        }
    }

    public void Patrol(FlockingBehavior flockingBehavior)
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        flockingBehavior.ApplyFlockingBehavior(targetWaypoint.position);

        
        if (Vector3.Distance(flockingBehavior.transform.position, targetWaypoint.position) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    public void AddSlime(FlockingBehavior slime)
    {
        if (!slimes.Contains(slime))
        {
            slimes.Add(slime);
        }
    }

    public void RemoveSlime(FlockingBehavior slime)
    {
        if (slimes.Contains(slime))
        {
            slimes.Remove(slime);
        }
    }
}




