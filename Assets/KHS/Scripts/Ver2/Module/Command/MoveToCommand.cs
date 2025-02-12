using UnityEngine;

public class MoveToCommand : ICommand
{
    private NPCController npcController;
    private Vector3 targetPosition;
    private bool finished = false;

    public MoveToCommand(NPCController _npc, Vector3 _targetPosition)
    {
        npcController = _npc;
        targetPosition = _targetPosition;
    }

    public void Execute()
    {
        Debug.Log($"{npcController.npcName} �� {targetPosition}�� ���� Destination��");
        npcController.animator.SetTrigger("Walk");
        npcController.agent.isStopped = false;  // NavMeshAgent ����
        npcController.agent.updatePosition = true;  // NavMesh ��ġ ������Ʈ ����
        npcController.agent.speed = npcController.walkSpeed;
        npcController.agent.SetDestination(targetPosition);
        if(Vector3.Distance(npcController.transform.position, targetPosition) <= npcController.agent.stoppingDistance)
        {
            Debug.Log($"{npcController.npcName} �� {targetPosition}�� ���� ����");
            finished = true;
        }
    }
    public bool IsFinished() => finished;

    public void End()
    {
        npcController.animator.ResetTrigger("Walk");
        npcController.agent.isStopped = true;  // NavMeshAgent ����
        npcController.agent.speed = 1f;
        npcController.agent.velocity = Vector3.zero;  // �̵� �ӵ� �ʱ�ȭ
        Debug.Log($"{npcController.npcName}�� {targetPosition}���� ���� �̵� �Ϸ�");
    }

}
