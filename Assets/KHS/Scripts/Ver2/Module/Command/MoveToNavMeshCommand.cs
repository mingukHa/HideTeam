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
        //Debug.Log("MoveToNavMeshCommand : Executed Call");
        //if (_npcController.curRoutine == null || _npcController.curRoutine.waypoints.Count == 0)
        //{
        //    _isFinished = true;
        //    return;
        //}

        //Vector3 targetPosition = _npcController.curRoutine.waypoints[_npcController.currentWaypointIndex];
        //_agent.SetDestination(targetPosition);

        if (_npcController.curRoutine == null || _npcController.curRoutine.waypoints.Count == 0)
        {
            _isFinished = true;
            return;
        }

        Vector3 targetPosition = _npcController.curRoutine.waypoints[_npcController.currentWaypointIndex];
        if (Vector3.Distance(_npcController.transform.position, targetPosition) < _agent.stoppingDistance)
        {
            _npcController.currentWaypointIndex = (_npcController.currentWaypointIndex + 1) % _npcController.curRoutine.waypoints.Count;
        }
        else
        {
            _agent.SetDestination(targetPosition);
        }

        _isFinished = _npcController.currentWaypointIndex == 0;
    }


    //public bool IsFinished()
    //{
    //    return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
    //}

    //public void End()
    //{
    //    Debug.Log("MoveToNavMeshCommand : Ended");
    //    _npcController.currentWaypointIndex = (_npcController.currentWaypointIndex + 1) % _npcController.curRoutine.waypoints.Count;
    //    _isFinished = true;
    //}
    public bool IsFinished()
    {
        return _isFinished;
    }

    public void End()
    {
        Debug.Log("MoveToNavMeshCommand: 이동 종료");
    }

}
