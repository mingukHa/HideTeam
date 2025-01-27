using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuardVision : MonoBehaviour
{
    [Header("시야 설정")]
    public float viewRadius = 10f; // 시야 범위 단위원 반지름
    [Range(0, 360)]
    public float viewAngle = 90f; // 시야 폭

    [Header("발각 설정")]
    public LayerMask targetMask; // 감지할 대상 레이어 (플레이어)
    public LayerMask obstructionMask; // 장애물 레이어 (벽 등)

    public bool istargetSituationDetected = false; // 플레이어가 발각되었는지 여부
    public Transform targetSituation; // 반응해야하는 타겟 Transform

    private List<Material> bodyMat;
    private float detectionTime = 2f; // 발각까지 걸리는 시간
    public float currentDetectionTime = 0f;

    private void Awake()
    {
        bodyMat = GetComponentInChildren<MeshRenderer>().materials.ToList();
    }

    private void Update()
    {
        // 플레이어 탐지
        DetecttargetSituation();
        ResponseSituation();
    }

    private void DetecttargetSituation()
    {
        istargetSituationDetected = false;

        // 시야 범위 안의 모든 대상 확인
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (var target in targetsInViewRadius)
        {
            // 대상이 플레이어인지 확인
            if (target.transform == targetSituation /*&& target.GetComponent<NPCStats>().isStunned*/)
            {
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

                // 시야 각도 내에 있는지 확인
                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                    // 장애물이 없는지 확인 (레이캐스트)
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        istargetSituationDetected = true; // 플레이어가 감지됨
                        Debug.DrawLine(transform.position, targetSituation.position, Color.red); // 디버그용 시야 선
                    }
                    else
                    {
                        Debug.DrawLine(transform.position, target.transform.position, Color.yellow); // 장애물이 있음
                    }
                }
            }
        }
    }
    private void ResponseSituation()
    {
        if (istargetSituationDetected)
        {
            if (currentDetectionTime >= detectionTime)
            {
                Debug.Log("플레이어 발각됨!");
                // 추가 처리: 알람 발동, 경비 상태 변화 등
                foreach (Material mat in bodyMat)
                {
                    mat.EnableKeyword("_EMISSION");
                }
                currentDetectionTime = detectionTime;
            }
            else
            {
                currentDetectionTime += Time.deltaTime;
            }
        }
        else
        {
            if (currentDetectionTime <= 0f)
            {
                foreach (Material mat in bodyMat)
                {
                    mat.DisableKeyword("_EMISSION");
                }
            }
            else
            {
                currentDetectionTime -= Time.deltaTime * 1.5f;
            }
        }
    }

    // 디버그: 시야를 시각화

    // 각도를 기준으로 방향 벡터 계산
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);

        if (istargetSituationDetected)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetSituation.position);
        }
    }
}
