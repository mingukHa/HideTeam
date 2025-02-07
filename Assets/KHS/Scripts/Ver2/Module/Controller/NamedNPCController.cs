using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NamedNPC))]
public class NamedNPCController : TNPCController
{
    public List<bool> eventTriggers;
    public List<PatrolRoute> testRoutine;
    public PatrolRoute currentRoute;

    [Header("루틴 동작 속성")]
    public float patrolSpeed = 2f;
    public float lookAroundDelay = 1f; // 회전 후 대기 시간

    private int currentWaypointIndex = 0;
    private bool isWaited = false;

    private void Start()
    {
        currentRoute = testRoutine[0];
    }
    public bool EventTargetRemove(int _eventIdx)
    {
        return eventTriggers[_eventIdx];
    }
    public void ChangeRoutine(int _nextRouteIdx)
    {
        currentRoute = testRoutine[_nextRouteIdx];
    }
    public bool Routine()
    {
        if (currentRoute == null || currentRoute.waypoints.Count == 0) return true;

        Vector3 targetPosition = currentRoute.waypoints[currentWaypointIndex];
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // 웨이포인트 방향으로 회전
        if (!isWaited)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);
        }

        // 웨이포인트로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        // 웨이포인트 도착 처리
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f && !isWaited)
        {
            StartCoroutine(WaitingCallRoutine());
            if (isWaited)
                return true;
        }
        return false;
    }
    private IEnumerator WaitingCallRoutine()
    {
        isWaited = true;
        float elapsedTime = 0f;

        while (elapsedTime < 5f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (currentWaypointIndex % 2 == 0)
        {
            if (!eventTriggers[0])
                Debug.Log("휴대폰 흘깃이는 애니메이션 재생");
            else
            {
                Debug.Log("휴대폰 확인 후 휴대폰 집어넣는 애니메이션 재생");
                yield break;
            }
        }
        else
        {
            Debug.Log("흡연 애니메이션 재생");
        }
        // 다음 웨이포인트로 이동
        currentWaypointIndex = (currentWaypointIndex + 1) % currentRoute.waypoints.Count;
        isWaited = false;
    }
}

