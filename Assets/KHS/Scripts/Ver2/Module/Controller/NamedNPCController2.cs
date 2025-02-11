using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NamedNPC2))]
public class NamedNPCController2 : NPCController
{
    public List<bool> events = new List<bool>();
    public List<RoutinePreset> routines = new List<RoutinePreset>();
    public RoutinePreset curRoutine;
    public int curRoutineIdx = 0;
    public int cureventIdx = 0;
    public int currentWaypointIndex = 0;
    public float routineSpeed = 7f;

    private NavMeshAgent agent;

    public override void Start()
    {
        base.Start();
        InitRoutine();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = routineSpeed;
        agent.stoppingDistance = 0.1f;

        //agent.speed = routineSpeed;
        //agent.stoppingDistance = 0.1f;
    }
    public void InitRoutine()
    {
        curRoutineIdx = 0;
        curRoutine = routines[curRoutineIdx];
    }
    public override bool Response()
    {
        return Routine();
    }
    public bool Routine()
    {
        if (curRoutine == null || curRoutine.waypoints.Count == 0) return true;

        Vector3 targetPosition = curRoutine.waypoints[currentWaypointIndex];
        //Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        if (Vector3.Distance(transform.position, targetPosition) < agent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % curRoutine.waypoints.Count;
        }
        else
        {
            agent.SetDestination(targetPosition);
        }

        return currentWaypointIndex == 0;
    }
    //// ��������Ʈ �������� ȸ��
    //Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);

    //    // ��������Ʈ�� �̵�
    //    transform.position = Vector3.MoveTowards(transform.position, targetPosition, routineSpeed * Time.deltaTime);

    //    // ��������Ʈ ���� ó��
    //    if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
    //    {
    //        currentWaypointIndex = (currentWaypointIndex + 1) % curRoutine.waypoints.Count;
    //    }
    //    if (currentWaypointIndex == 0)
    //        return true;

    //    return false;
    //}
    public bool EventTrigger(int _eventIdx)
    {
        return events[_eventIdx];
    }
    public void ChangeRoutine()
    {
        int tempCRIdx = (curRoutineIdx + 1) % routines.Count;
        Debug.Log("ChangeRoutine Index : " + tempCRIdx);
        curRoutineIdx = tempCRIdx;
        curRoutine = routines[tempCRIdx];
    }
}

