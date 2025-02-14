using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using static EventManager;


[RequireComponent(typeof(NPCStateMachine)), RequireComponent(typeof(RoutineInvoker))]
public abstract class NPCController : MonoBehaviour
{
    public NPCType npcType;

    [Header("NPC 정보")]
    public string npcName = string.Empty;
    public NPCStateMachine stateMachine;
    public RoutineInvoker routineInvoker { get; private set; }

    [Header("이동 속성")]
    public float rotationSpeed = 1.0f;
    public float walkSpeed = 2.0f;
    public float runSpeed = 3.0f;

    [Header("시야 설정")]
    public float viewRadius = 10f; // 시야 범위 단위원 반지름
    [Range(0, 360)]
    public float viewAngle = 90f; // 시야 폭

    [Header("발각 설정")]
    public LayerMask targetMask; // 감지할 대상 레이어 (플레이어)
    public LayerMask obstructionMask; // 장애물 레이어 (벽 등)
    public bool isDetected = false;
    public List<Transform> targetTr; // 반응해야하는 타겟 Transform
    public List<Vector3> targetVec;

    [Header("애니메이터 및 NavMesh Agent")]
    public NavMeshAgent agent;
    public Animator animator;

    [Header("이벤트 설정")]
    public List<GameEventType> eventFlags = new List<GameEventType>();
    public List<string> actionNames = new List<string>();
    private Dictionary<GameEventType, Action> eventActions = new Dictionary<GameEventType, Action>();

    private void Awake()
    {
        npcName = transform.name;
        stateMachine = GetComponent<NPCStateMachine>();
        routineInvoker = GetComponent<RoutineInvoker>();
        npcType = GetComponent<NPCType>();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        Debug.Log($"{gameObject.name} - NPCController Awake() 실행됨, npcType: {npcType}");
    }
    public virtual void Start()
    {
        if (routineInvoker.npcRoutines.Count != 0)
        {
            stateMachine.ChangeState(new IdleState(this));
        }
        agent.speed = walkSpeed;
        agent.angularSpeed = 200f;
        agent.stoppingDistance = 0.8f;
        UpdateTargetInfo();

    }
    protected virtual void OnEnable()
    {
        InitializeEventActions();
        SubscribeEvents();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeEvents();
    }

    // 이벤트 동작 매핑
    private void InitializeEventActions()
    {
        eventActions.Clear();
        for (int i = 0; i < eventFlags.Count && i < actionNames.Count; i++)
        {
            string methodName = actionNames[i];
            Action action = (Action)Delegate.CreateDelegate(typeof(Action), this, methodName, false);
            if (action != null)
            {
                eventActions[eventFlags[i]] = action;
            }
            else
            {
                Debug.LogWarning($"{methodName} 메서드를 찾을 수 없습니다! {npcName}에 정의되어 있어야 합니다.");
            }
        }
    }

    // 이벤트 등록
    private void SubscribeEvents()
    {
        foreach (var kvp in eventActions)
        {
            EventManager.Subscribe(kvp.Key, kvp.Value);
        }
    }

    // 이벤트 해제
    private void UnsubscribeEvents()
    {
        foreach (var kvp in eventActions)
        {
            EventManager.Unsubscribe(kvp.Key, kvp.Value);
        }
    }

    public void UpdateTargetInfo()
    {
        targetVec.Clear();
        foreach (Transform tr in targetTr)
        {
            targetVec.Add(tr.position);
        }
    }
    public bool HasDetectedTarget()
    {
        Debug.Log("HasDetectedTarget Call");
        isDetected = false;

        // 시야 범위 안의 모든 대상 확인
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (var target in targetsInViewRadius)
        {
            // 대상이 타겟 상황인지 확인
            if (target.GetComponent<SusState>().IsSuspicious == true)
            {
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

                // 시야 각도 내에 있는지 확인
                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                    // 장애물이 없는지 확인 (레이캐스트)
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        targetTr.Add(target.transform);
                        isDetected = true;
                        Debug.DrawLine(transform.position, target.transform.position, Color.red); // 디버그용 시야 선
                    }
                    else
                    {
                        Debug.DrawLine(transform.position, target.transform.position, Color.yellow); // 장애물이 있음
                    }
                }
            }
        }

        if (isDetected)
        {
            Debug.Log("의심 상황 발각!");
            // 추가 처리: 알람 발동, 경비 상태 변화 등
        }
        else
        {
            Debug.Log("의심 상황 미발각");
        }
        return isDetected;
    }
    public bool CurrentRoutineEnd()
    {
        return routineInvoker.RoutineEnd();
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
    }
}
