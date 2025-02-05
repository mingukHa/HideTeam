using UnityEngine;

public class TNPCController : MonoBehaviour
{
    private StateController stateController;
    

    private void Start()
    { 
        stateController = GetComponent<StateController>();

        // �ʱ� ����: Idle
        stateController.ChangeState(new IdleState(this));

    }

    public void ChangeState(NPCState _State)
    {
        stateController.ChangeState(_State);
    }

    public void MoveToTarget()
    {

    }

    // Ž��, �߰� ���� �޼���
    public void StartDetection() { Debug.Log("Ž�� ��� Ȱ��ȭ"); }
    public void EndDetection() { Debug.Log("Ž�� ��� ��Ȱ��ȭ"); }

    public void StartChase() { Debug.Log("�߰� ��� Ȱ��ȭ"); }
    public void EndChase() { Debug.Log("�߰� ��� ��Ȱ��ȭ"); }

    public void StartPatrol() { Debug.Log("���� ��� Ȱ��ȭ"); }
    public void EndPatrol() { Debug.Log("���� ��� ��Ȱ��ȭ"); }

    // ���� ������ ���� ���� üũ
    public bool CanDetectPlayer() { return false; }
    public bool PlayerSpotted() { return false; }
    public bool PlayerVisible() { return false; }
    public bool CanPatrol() { return false; }

}
