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
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    private NavMeshAgent agent;
    private FlockingBehavior flockingBehavior;
    private GroupManager groupManager;
    private Vector3 initialPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        flockingBehavior = GetComponent<FlockingBehavior>();
        groupManager = FindObjectOfType<GroupManager>();
        initialPosition = transform.position;

        if (agent == null) Debug.LogError("NavMeshAgent component missing!");
        if (flockingBehavior == null) Debug.LogError("FlockingBehavior component missing!");
        if (groupManager == null) Debug.LogError("GroupManager not found!");

        currentState = State.Patrol;
    }

    void Update()
    {
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
            flockingBehavior.IsFlocking = true;
            groupManager.Patrol(flockingBehavior);
        }

        if (Vector3.Distance(transform.position, player.position) < chaseDistance)
        {
            currentState = State.Chase;
        }
    }

    private void Chase()
    {
        flockingBehavior.IsFlocking = false;
        agent.speed = chaseSpeed;

        if (player != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);

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
        agent.isStopped = true;
        Debug.Log("Attacking the player!");

        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            currentState = State.Chase;
            agent.isStopped = false;
        }
    }

    private void Dead()
    {
        Debug.Log("Slime is dead!");
        Destroy(gameObject);
    }

    private void ReturnToPatrol()
    {
        agent.speed = patrolSpeed;

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(initialPosition);
        }

        if (Vector3.Distance(transform.position, initialPosition) < 1f)
        {
            currentState = State.Patrol;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            currentState = State.Dead;
        }
    }
}








