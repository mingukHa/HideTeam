using UnityEngine;
using UnityEngine.AI;

public class MoveToNavMeshCommand : ICommand
{
    private NamedNPCController2 _npcController;
    private NavMeshAgent _agent;
    private bool _isFinished;

    public MoveToNavMeshCommand(NamedNPCController2 npc)
    {
        _npcController = npc;
        _agent = npc.GetComponent<NavMeshAgent>();
        _isFinished = false;
    }

    public void Execute()
    {
        Debug.Log("MoveToNavMeshCommand : Executed Call");
        if (_npcController.curRoutine == null || _npcController.curRoutine.waypoints.Count == 0)
        {
            _isFinished = true;
            return;
        }

        Vector3 targetPosition = _npcController.curRoutine.waypoints[_npcController.currentWaypointIndex];
        _agent.SetDestination(targetPosition);
    }

    public bool IsFinished()
    {
        return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
    }

    public void End()
    {
        Debug.Log("MoveToNavMeshCommand : Ended");
        _npcController.currentWaypointIndex = (_npcController.currentWaypointIndex + 1) % _npcController.curRoutine.waypoints.Count;
        _isFinished = true;
    }

}
