using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentDebugger : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target; // 目标物体

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on this GameObject!");
            return;
        }
    }

    void Update()
    {
        if (target != null)
        {
            // 设置目标点
            NavMeshHit hit;
            if (NavMesh.SamplePosition(target.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                Debug.Log($"Setting destination to: {hit.position}");
            }
            else
            {
                Debug.LogWarning("Target is not on NavMesh!");
            }
        }

        // 检测路径状态
        if (agent.hasPath)
        {
            Debug.Log($"Path Status: {agent.pathStatus}");
            DrawPath(agent.path); // 绘制路径
        }
        else
        {
            Debug.Log("Agent has no path!");
        }

        // 检查是否到达目标
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Debug.Log("Agent has reached the destination!");
        }
    }

    void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) return;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.green, 0.1f); // 绘制绿色路径线
        }
    }
}
