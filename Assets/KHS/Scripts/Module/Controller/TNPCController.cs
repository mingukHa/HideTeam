using UnityEngine;

[RequireComponent(typeof(NPCStateMachine)),RequireComponent(typeof(CommandInvoker))]
public class TNPCController : MonoBehaviour
{
    public NPCType npcType;

    [Header("NPC 정보")]
    public string npcName = string.Empty;
    public NPCStateMachine stateMachine;
    public CommandInvoker Invoker { get; private set; }

    [Header("이동 속성")]
    public float rotationSpeed = 1.0f;
    public float walkSpeed = 4.0f;

    [Header("시야 설정")]
    public float viewRadius = 10f; // 시야 범위 단위원 반지름
    [Range(0, 360)]
    public float viewAngle = 90f; // 시야 폭

    [Header("발각 설정")]
    public LayerMask targetMask; // 감지할 대상 레이어 (플레이어)
    public LayerMask obstructionMask; // 장애물 레이어 (벽 등)
    public float detectionTime = 2f; // 발각까지 걸리는 시간
    public float currentDetectionTime = 0f;
    public bool istargetSituationDetected = false;
    public Transform targetSituation; // 반응해야하는 타겟 Transform
    public Vector3 _target = Vector3.zero;


    private void Awake()
    {
        stateMachine = GetComponent<NPCStateMachine>();
        Invoker = GetComponent<CommandInvoker>();
        npcType = GetComponent<NPCType>();

        Debug.Log($"{gameObject.name} - Awake() 실행됨, npcType: {npcType}");
    }
    public virtual void Start()
    {
        stateMachine.ChangeState(new IdleState(this));
        _target = targetSituation.transform.position;

    }

    public bool MoveToTarget(Vector3 _target)
    {
        if (targetSituation == null) return true;

        Vector3 directionToTarget = (_target - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);

        transform.position = Vector3.MoveTowards(transform.position, _target, walkSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _target) <= 0.5f)
            return true;
        else
            return false;
    }

    public bool HasDetectedTarget()
    {
        Debug.Log("탐지중...");
        istargetSituationDetected = false;
        bool tempReturn = false;

        // 시야 범위 안의 모든 대상 확인
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (var target in targetsInViewRadius)
        {
            // 대상이 타겟 상황인지 확인
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
                        Debug.DrawLine(transform.position, targetSituation.position, Color.red); // 디버그용 시야 선
                        _target = targetSituation.position;
                        istargetSituationDetected = true;
                    }
                    else
                    {
                        Debug.DrawLine(transform.position, target.transform.position, Color.yellow); // 장애물이 있음
                    }
                }
            }
        }

        if (istargetSituationDetected)
        {
            if (currentDetectionTime >= detectionTime)
            {
                Debug.Log("플레이어 발각됨!");
                // 추가 처리: 알람 발동, 경비 상태 변화 등
                tempReturn = true;
                currentDetectionTime = detectionTime;
            }
            else
            {
                currentDetectionTime += Time.deltaTime;
            }
        }
        else
        {
            currentDetectionTime -= Time.deltaTime;

            if (currentDetectionTime < 0)
            {
                Debug.Log("플레이어 놓침!");
                currentDetectionTime = 0;
                tempReturn = false;
            }
        }
        return tempReturn;
    }

    public Vector3 GetTargetPosition()
    {
        return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }

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