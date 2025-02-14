using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class NPCPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    public PatrolRoute patrolRoute;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(patrolRoute.waypoints[currentWaypointIndex]);
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            NextWaypoint();
            
        }
    }
    void NextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolRoute.waypoints.Count;
        agent.SetDestination(patrolRoute.waypoints[currentWaypointIndex]);

    }
}
