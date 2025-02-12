using UnityEngine;

public class WaitState : NPCState
{
    private RoutineInvoker routineInvoker;

    public WaitState(NPCController npc) : base(npc)
    {
        routineInvoker = npc.GetComponent<RoutineInvoker>();
    }

    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}�� ��� ���¿� ����");
        _npcController.agent.isStopped = true;  // NavMeshAgent ����
        _npcController.agent.velocity = Vector3.zero;  // �̵� �ӵ� �ʱ�ȭ
        _npcController.agent.updatePosition = false;  // NavMesh ��ġ ������Ʈ ����
        _npcController.agent.updateRotation = false;  // NavMesh ȸ�� ����
        _npcController.animator.SetTrigger("Idle");
    }

    public override void Update()
    {

    }
}
