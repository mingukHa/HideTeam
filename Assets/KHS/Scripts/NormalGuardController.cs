using UnityEngine;

public class NormalGuardController : NPCController
{
    public AIConState currentState = AIConState.Idle;
    public PatrolRoute patrolRoute;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    private int currentWaypointIndex = 0;
    private Transform playerTr;

    private void Start()
    {
        currentState = AIConState.Idle;
    }

    private void Update()
    {
        switch(currentState)
        {
            case AIConState.Idle:
                Patrol();
                break;

            case AIConState.SuspectDetected:
                ChaseSuspect();
                break;
        }
    }

    private void Patrol()
    {
        if (patrolRoute == null || patrolRoute.waypoints.Count == 0) return;

        Vector3 target = patrolRoute.waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, patrolSpeed*Time.deltaTime);

        if(Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % patrolRoute.waypoints.Count;
        }
    }
    private void ChaseSuspect()
    {
        if (playerTr == null) return;
        
        transform.position = Vector3.MoveTowards(transform.position, playerTr.position, chaseSpeed*Time.deltaTime);
    }

    public void ChangeState(AIConState newState, Transform detectedSuspect = null)
    {
        currentState = newState;

        if(newState == AIConState.SuspectDetected && detectedSuspect != null)
        {
            playerTr = detectedSuspect;
        }

        Debug.Log($"NPC State Changed : {currentState}");
        if (currentState == AIConState.SuspectDetected)
        {
            Debug.Log($"Chasing Player at position: {playerTr.position}");
        }
    }
}
