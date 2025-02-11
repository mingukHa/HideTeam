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

    public bool isLookingAround = false;
    public float lookAroundDelay = 1f; // 회전 후 대기 시간

    public override void Start()
    {
        base.Start();
        InitRoutine();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = routineSpeed;
        agent.stoppingDistance = 0.1f;
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

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % curRoutine.waypoints.Count;
            agent.SetDestination(curRoutine.waypoints[currentWaypointIndex]);
        }
        if (currentWaypointIndex == 0)
            return true;
        return false;
    }

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

        // 웨이포인트 인덱스를 0으로 초기화
        currentWaypointIndex = 0;

        // 변경된 루틴의 첫 번째 웨이포인트로 즉시 이동
        if (curRoutine.waypoints.Count > 0)
        {
            agent.SetDestination(curRoutine.waypoints[0]);
        }
    }
}

