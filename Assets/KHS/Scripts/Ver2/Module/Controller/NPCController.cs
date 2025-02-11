using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NPCStateMachine2)), RequireComponent(typeof(CommandInvoker))]
public abstract class NPCController : MonoBehaviour
{
    public NPCType2 npcType;

    [Header("NPC ����")]
    public string npcName = string.Empty;
    public NPCStateMachine2 stateMachine;
    public CommandInvoker Invoker { get; private set; }
    public RoutineInvoker routineInvoker { get; private set; }

    [Header("�̵� �Ӽ�")]
    public float rotationSpeed = 1.0f;
    public float walkSpeed = 4.0f;

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

    public NavMeshAgent agent;
    public Animator animator;

    private void Awake()
    {
        npcName = transform.name;
        stateMachine = GetComponent<NPCStateMachine2>();
        Invoker = GetComponent<CommandInvoker>();
        routineInvoker = GetComponent<RoutineInvoker>();
        npcType = GetComponent<NPCType2>();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        Debug.Log($"{gameObject.name} - NPCController Awake() �����, npcType: {npcType}");
    }
    public virtual void Start()
    {
        if (routineInvoker.npcRoutine != null)
        {
            stateMachine.ChangeState(new RoutineState(this));
        }
        else
        {
            stateMachine.ChangeState(new IdleState2(this));
        }
        agent.speed = walkSpeed;
        agent.stoppingDistance = 0.8f;
        UpdateTargetInfo();
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
                        Debug.DrawLine(transform.position, target.transform.position, Color.red); // ����׿� �þ� ��
                        targetTr.Add(target.transform);
                        UpdateTargetInfo();
                        isDetected = true;
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
    public abstract bool Response();

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
    public IEnumerator TalkCoroutine()
    {
        Debug.Log("TalkCoroutine");
        yield return null;
    }
}
