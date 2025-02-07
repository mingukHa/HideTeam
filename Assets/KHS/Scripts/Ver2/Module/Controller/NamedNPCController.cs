using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NamedNPC))]
public class NamedNPCController : TNPCController
{
    public List<bool> eventTriggers;
    public List<PatrolRoute> testRoutine;
    public PatrolRoute currentRoute;

    [Header("��ƾ ���� �Ӽ�")]
    public float patrolSpeed = 2f;
    public float lookAroundDelay = 1f; // ȸ�� �� ��� �ð�

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

        // ��������Ʈ �������� ȸ��
        if (!isWaited)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);
        }

        // ��������Ʈ�� �̵�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        // ��������Ʈ ���� ó��
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
                Debug.Log("�޴��� ����̴� �ִϸ��̼� ���");
            else
            {
                Debug.Log("�޴��� Ȯ�� �� �޴��� ����ִ� �ִϸ��̼� ���");
                yield break;
            }
        }
        else
        {
            Debug.Log("�� �ִϸ��̼� ���");
        }
        // ���� ��������Ʈ�� �̵�
        currentWaypointIndex = (currentWaypointIndex + 1) % currentRoute.waypoints.Count;
        isWaited = false;
    }
}

