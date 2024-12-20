using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeFSM : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack, Dead, ReturnToPatrol }
    public State currentState;

    public Transform player;
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float health = 100f;

    private NavMeshAgent agent;
    private GroupManager groupManager;
    private Vector3 initialPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        groupManager = FindObjectOfType<GroupManager>();
        initialPosition = transform.position;

        if (agent == null) Debug.LogError("NavMeshAgent component missing!");
        if (groupManager == null) Debug.LogError("GroupManager not found!");

        currentState = State.Patrol; // Start in patrol state
    }

    void Update()
    {
        // Execute behavior based on the current state
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Dead:
                Dead();
                break;
            case State.ReturnToPatrol:
                ReturnToPatrol();
                break;
        }
    }

    private void Patrol()
    {
        if (groupManager != null)
        {
            groupManager.Patrol(agent); // Patrol as a group
        }

        // Transition to Chase if the player is within range
        if (Vector3.Distance(transform.position, player.position) < chaseDistance)
        {
            currentState = State.Chase;
        }
    }

    private void Chase()
    {
        if (player != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position); // Move towards the player

            // Transition to Attack if close, or return to patrol if too far
            if (Vector3.Distance(transform.position, player.position) < attackDistance)
            {
                currentState = State.Attack;
            }
            else if (Vector3.Distance(transform.position, player.position) > chaseDistance)
            {
                currentState = State.ReturnToPatrol;
            }
        }
    }

    private void Attack()
    {
        agent.isStopped = true; // Stop movement
        Debug.Log("Attacking the player!");

        // Transition back to Chase if the player moves out of range
        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            currentState = State.Chase;
            agent.isStopped = false;
        }
    }

    private void Dead()
    {
        Debug.Log("Slime is dead!"); // Log death
        Destroy(gameObject); // Remove the slime
    }

    private void ReturnToPatrol()
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(initialPosition); // Move back to the initial position
        }

        // Transition to Patrol state when close to the initial position
        if (Vector3.Distance(transform.position, initialPosition) < 1f)
        {
            currentState = State.Patrol;
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentState == State.Dead) return; // Ignore if already dead

        health -= damage; // Reduce health
        Debug.Log($"{gameObject.name} took {damage} damage! Remaining health: {health}");

        // Transition to Dead state if health drops to zero or below
        if (health <= 0)
        {
            currentState = State.Dead;
        }
    }
}









