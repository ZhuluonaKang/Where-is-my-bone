using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatFlocking : MonoBehaviour
{
    [Header("Flocking Settings")]
    public GameObject batPrefab;
    public int batCount = 5;
    public float spawnRadius = 5f;
    public float speed = 5f;
    public float neighborRadius = 3f;
    public float separationRadius = 1f;

    [Header("Weights")]
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float separationWeight = 2f;
    public float targetWeight = 1.5f;

    [Header("Target Settings")]
    public Transform[] waypoints;

    private int currentWaypointIndex = 0;
    private List<Bat> bats = new List<Bat>();

    void Start()
    {
        // Spawn bats randomly within the radius
        for (int i = 0; i < batCount; i++)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
            GameObject batObj = Instantiate(batPrefab, spawnPos, Quaternion.identity);
            batObj.transform.parent = transform;
            bats.Add(new Bat(batObj));
        }
    }

    void Update()
    {
        // Update the target waypoint and bat behaviors
        UpdateWaypoint();

        foreach (var bat in bats)
        {
            // Calculate behaviors and update bat position
            Vector3 alignment = ComputeAlignment(bat) * alignmentWeight;
            Vector3 cohesion = ComputeCohesion(bat) * cohesionWeight;
            Vector3 separation = ComputeSeparation(bat) * separationWeight;
            Vector3 targetDirection = (waypoints[currentWaypointIndex].position - bat.Position).normalized * targetWeight;

            Vector3 velocity = alignment + cohesion + separation + targetDirection;
            velocity = velocity.normalized * speed;

            bat.UpdatePosition(velocity, Time.deltaTime);
        }
    }

    void UpdateWaypoint()
    {
        // Switch to the next waypoint if the first bat reaches it
        if (Vector3.Distance(bats[0].Position, waypoints[currentWaypointIndex].position) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    Vector3 ComputeAlignment(Bat bat)
    {
        // Align bat's direction with nearby bats
        Vector3 averageVelocity = Vector3.zero;
        int neighborCount = 0;

        foreach (var otherBat in bats)
        {
            if (otherBat == bat) continue;
            if (Vector3.Distance(bat.Position, otherBat.Position) < neighborRadius)
            {
                averageVelocity += otherBat.Velocity;
                neighborCount++;
            }
        }

        return neighborCount > 0 ? (averageVelocity / neighborCount).normalized : Vector3.zero;
    }

    Vector3 ComputeCohesion(Bat bat)
    {
        // Move bat towards the center of nearby bats
        Vector3 centerOfMass = Vector3.zero;
        int neighborCount = 0;

        foreach (var otherBat in bats)
        {
            if (otherBat == bat) continue;
            if (Vector3.Distance(bat.Position, otherBat.Position) < neighborRadius)
            {
                centerOfMass += otherBat.Position;
                neighborCount++;
            }
        }

        return neighborCount > 0 ? (centerOfMass / neighborCount - bat.Position).normalized : Vector3.zero;
    }

    Vector3 ComputeSeparation(Bat bat)
    {
        // Avoid crowding other bats
        Vector3 separationForce = Vector3.zero;

        foreach (var otherBat in bats)
        {
            if (otherBat == bat) continue;
            float distance = Vector3.Distance(bat.Position, otherBat.Position);
            if (distance < separationRadius)
            {
                separationForce += (bat.Position - otherBat.Position) / distance;
            }
        }

        return separationForce.normalized;
    }

    private class Bat
    {
        public GameObject GameObject { get; }
        public Vector3 Position => GameObject.transform.position;
        public Vector3 Velocity { get; private set; }

        public Bat(GameObject gameObject)
        {
            GameObject = gameObject;
            Velocity = Vector3.zero;
        }

        // Updates the bat's position and rotation based on its velocity
        public void UpdatePosition(Vector3 newVelocity, float deltaTime)
        {
            Velocity = newVelocity;
            GameObject.transform.position += Velocity * deltaTime;
            if (Velocity != Vector3.zero)
                GameObject.transform.rotation = Quaternion.LookRotation(Velocity);
        }
    }
}


