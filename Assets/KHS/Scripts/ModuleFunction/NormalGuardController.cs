using UnityEngine;
using System.Collections;

public class NormalGuardController : NPCController
{
    [Header("순찰 설정")]
    public PatrolRoute patrolRoute;
    public float patrolSpeed = 2f;
    public float rotationSpeed = 2f; // 회전 속도
    public float lookAroundDelay = 1f; // 회전 후 대기 시간

    [Header("추격 설정")]
    public float chaseSpeed = 4f;

    private int currentWaypointIndex = 0;
    private bool isLookingAround = false;

    public override void PatrolOrMove()
    {
        if (data.currentState != AIConState.Patrol || patrolRoute == null || patrolRoute.waypoints.Count == 0) return;

        Vector3 targetPosition = patrolRoute.waypoints[currentWaypointIndex];
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // 웨이포인트 방향으로 회전
        if (!isLookingAround)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);
        }

        // 웨이포인트로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        // 웨이포인트 도착 처리
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f && !isLookingAround)
        {
            StartCoroutine(LookAroundRoutine());
        }
    }

    public override void ChaseSuspect()
    {
        if (data.currentState != AIConState.SuspectDetected || targetSituation == null) return;

        Vector3 directionToTarget = (targetSituation.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);

        transform.position = Vector3.MoveTowards(transform.position, targetSituation.position, chaseSpeed * Time.deltaTime);
    }

    private IEnumerator LookAroundRoutine()
    {
        isLookingAround = true;

        for (int i = 0; i < 2; i++) // 2번 회전
        {
            float randomYaw = Random.Range(-60f, 60f);
            Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y + randomYaw, 0);

            float elapsedTime = 0f;
            Quaternion startRotation = transform.rotation;

            while (elapsedTime < 1f)
            {
                transform.rotation = Quaternion.Slerp(startRotation, newRotation, elapsedTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = newRotation;
            yield return new WaitForSeconds(lookAroundDelay); // 일정 시간 대기
        }

        // 다음 웨이포인트로 이동
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolRoute.waypoints.Count;
        isLookingAround = false;
    }
}
