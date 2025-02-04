using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public NPCData data;

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

    private List<Material> bodyMat;

    private void Awake()
    {
        data = GetComponent<NPCData>();
        foreach(AIModule aiModule in data.aiModules)
        {
            aiModule.Initialize();
        }
    }
    private void Start()
    {
        bodyMat = GetComponentInChildren<MeshRenderer>().materials.ToList();
    }
    private void Update()
    {
        foreach (AIModule aiModule in data.aiModules)
        {
            aiModule.ExecuteBehaviors(this);
        }
    }
    public virtual void PatrolOrMove()
    {

    }

    public virtual void ChaseSuspect()
    {

    }

    public virtual void ResponseSituation()
    {
        if (istargetSituationDetected)
        {
            if (currentDetectionTime >= detectionTime)
            {
                Debug.Log("�÷��̾� �߰���!");
                // �߰� ó��: �˶� �ߵ�, ��� ���� ��ȭ ��
                ChangeState(AIConState.SuspectDetected, targetSituation);
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
                ChangeState(AIConState.Patrol);
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

    public void Detect()
    {
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
                        istargetSituationDetected = true; // Ÿ�� ��Ȳ�� ������
                        Debug.DrawLine(transform.position, targetSituation.position, Color.red); // ����׿� �þ� ��
                    }
                    else
                    {
                        Debug.DrawLine(transform.position, target.transform.position, Color.yellow); // ��ֹ��� ����
                    }
                }
            }
        }
    }
    public void ChangeState(AIConState newState, Transform detectedSuspect = null)
    {
        data.currentState = newState;

        if (newState == AIConState.SuspectDetected && detectedSuspect != null)
        {
            targetSituation = detectedSuspect;
        }

        Debug.Log($"NPC State Changed : {data.currentState}");
    }


    // �����: �þ߸� �ð�ȭ

    // ������ �������� ���� ���� ���
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
