using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TNPCController : MonoBehaviour
{
    [Header("NPC ����")]
    public string npcName = string.Empty;
    public NPCStateMachine stateMachine;
    public CommandInvoker Invoker { get; private set; }

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
    public float detectionTime = 2f; // �߰����� �ɸ��� �ð�
    public float currentDetectionTime = 0f;
    public bool istargetSituationDetected = false; // Ÿ�� ��Ȳ�� �����Ǿ����� ����
    public Transform targetSituation; // �����ؾ��ϴ� Ÿ�� Transform


    private void Awake()
    {
        stateMachine = GetComponent<NPCStateMachine>();
        Invoker = GetComponent<CommandInvoker>();
    }
    private void Start()
    {
        stateMachine.ChangeState(new IdleState(this));
        
    }

    public void MoveToTarget(Vector3 target)
    {
        if (target == null) return;

        Vector3 directionToTarget = (target - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);

        transform.position = Vector3.MoveTowards(transform.position, target, walkSpeed * Time.deltaTime);
    }

    public bool HasDetectedTarget()
    {
        Debug.Log("HDTȣ���");
        istargetSituationDetected = false;

        // �þ� ���� ���� ��� ��� Ȯ��
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (var target in targetsInViewRadius)
        {
            // ����� Ÿ�� ��Ȳ���� Ȯ��
            if (target.transform == targetSituation /*&& target.GetComponent<NPCStats>().isStunned*/)
            {
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

                // �þ� ���� ���� �ִ��� Ȯ��
                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                    // ��ֹ��� ������ Ȯ�� (����ĳ��Ʈ)
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        Debug.DrawLine(transform.position, targetSituation.position, Color.red); // ����׿� �þ� ��
                        istargetSituationDetected = true;
                    }
                    else
                    {
                        Debug.DrawLine(transform.position, target.transform.position, Color.yellow); // ��ֹ��� ����
                    }
                }
            }
        }

        if (istargetSituationDetected)
        {
            if (currentDetectionTime >= detectionTime)
            {
                Debug.Log("�÷��̾� �߰���!");
                // �߰� ó��: �˶� �ߵ�, ��� ���� ��ȭ ��

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
                currentDetectionTime = 0;
        }

        return istargetSituationDetected;
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