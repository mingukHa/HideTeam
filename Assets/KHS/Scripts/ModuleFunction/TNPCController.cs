using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TNPCController : MonoBehaviour
{
    private StateController stateController;
    private NavMeshAgent agent;
    public Transform target; // �÷��̾� ��ġ

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateController = GetComponent<StateController>();

        // �ʱ� ����: Idle
        stateController.ChangeState(new IdleState(this));

        // ���� �ð����� ���� ������Ʈ (���� ���ӿ����� �ʿ� ���� ���� ����)
        StartCoroutine(StateUpdateLoop());
    }

    public void ChangeState(NPCState newState)
    {
        stateController.ChangeState(newState);
    }

    public void MoveToTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    // Ž��, �߰� ���� �޼���
    public void StartDetection() { Debug.Log("Ž�� ��� Ȱ��ȭ"); }
    public void EndDetection() { Debug.Log("Ž�� ��� ��Ȱ��ȭ"); }

    public void StartChase() { Debug.Log("�߰� ��� Ȱ��ȭ"); }
    public void EndChase() { Debug.Log("�߰� ��� ��Ȱ��ȭ"); }

    public void StartPatrol() { Debug.Log("���� ��� Ȱ��ȭ"); }
    public void EndPatrol() { Debug.Log("���� ��� ��Ȱ��ȭ"); }

    // ���� ������ ���� ���� üũ
    public bool CanDetectPlayer() { return Random.value > 0.7f; } // ����: 30% Ȯ���� Ž�� ����
    public bool PlayerSpotted() { return Random.value > 0.5f; } // ����: 50% Ȯ���� �߰� ����
    public bool PlayerVisible() { return Random.value > 0.2f; } // ����: 80% Ȯ���� �߰� ����
    public bool CanPatrol() { return Random.value > 0.4f; }

    // ���� ������Ʈ ���� (�׽�Ʈ��)
    private IEnumerator StateUpdateLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // 2�ʸ��� ���� ������Ʈ
            stateController.UpdateState();
        }
    }
}
