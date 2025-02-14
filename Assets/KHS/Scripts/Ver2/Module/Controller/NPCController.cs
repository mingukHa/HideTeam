using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using static EventManager;


[RequireComponent(typeof(NPCStateMachine)), RequireComponent(typeof(RoutineInvoker))]
public abstract class NPCController : MonoBehaviour
{
    public NPCType npcType;

    [Header("NPC ����")]
    public string npcName = string.Empty;
    public NPCStateMachine stateMachine;
    public RoutineInvoker routineInvoker { get; private set; }

    [Header("�̵� �Ӽ�")]
    public float rotationSpeed = 1.0f;
    public float walkSpeed = 2.0f;
    public float runSpeed = 3.0f;

    [Header("�þ� ����")]
    public float viewRadius = 10f; // �þ� ���� ������ ������
    [Range(0, 360)]
    public float viewAngle = 90f; // �þ� ��

    [Header("�߰� ����")]
    public LayerMask targetMask; // ������ ��� ���̾� (�÷��̾�)
    public LayerMask obstructionMask; // ��ֹ� ���̾� (�� ��)
    public bool isDetected = false;
    public List<Transform> targetTr; // �����ؾ��ϴ� Ÿ�� Transform
    public List<Vector3> targetVec;

    [Header("�ִϸ����� �� NavMesh Agent")]
    public NavMeshAgent agent;
    public Animator animator;

    [Header("�̺�Ʈ ����")]
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

        Debug.Log($"{gameObject.name} - NPCController Awake() �����, npcType: {npcType}");
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

    // �̺�Ʈ ���� ����
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
                Debug.LogWarning($"{methodName} �޼��带 ã�� �� �����ϴ�! {npcName}�� ���ǵǾ� �־�� �մϴ�.");
            }
        }
    }

    // �̺�Ʈ ���
    private void SubscribeEvents()
    {
        foreach (var kvp in eventActions)
        {
            EventManager.Subscribe(kvp.Key, kvp.Value);
        }
    }

    // �̺�Ʈ ����
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

        // �þ� ���� ���� ��� ��� Ȯ��
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (var target in targetsInViewRadius)
        {
            // ����� Ÿ�� ��Ȳ���� Ȯ��
            if (target.GetComponent<SusState>().IsSuspicious == true)
            {
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

                // �þ� ���� ���� �ִ��� Ȯ��
                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                    // ��ֹ��� ������ Ȯ�� (����ĳ��Ʈ)
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        targetTr.Add(target.transform);
                        isDetected = true;
                        Debug.DrawLine(transform.position, target.transform.position, Color.red); // ����׿� �þ� ��
                    }
                    else
                    {
                        Debug.DrawLine(transform.position, target.transform.position, Color.yellow); // ��ֹ��� ����
                    }
                }
            }
        }

        if (isDetected)
        {
            Debug.Log("�ǽ� ��Ȳ �߰�!");
            // �߰� ó��: �˶� �ߵ�, ��� ���� ��ȭ ��
        }
        else
        {
            Debug.Log("�ǽ� ��Ȳ �̹߰�");
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
