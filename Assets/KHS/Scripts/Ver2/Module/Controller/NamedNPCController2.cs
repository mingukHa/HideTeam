using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void Start()
    {
        stateMachine.ChangeState(new IdleState2(this));
        UpdateTargetInfo();
        InitRoutine();
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
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // 웨이포인트 방향으로 회전
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);

        // 웨이포인트로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, routineSpeed * Time.deltaTime);

        // 웨이포인트 도착 처리
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % curRoutine.waypoints.Count;
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
    }
}

