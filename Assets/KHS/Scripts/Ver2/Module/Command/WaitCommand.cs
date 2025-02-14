using UnityEngine;

public class WaitCommand : ICommand
{
    private NPCController npcController;
    private float waitTime;
    private float elapsedTime = 0f;
    private bool finished = false;

    public WaitCommand(NPCController _npc, float _waitTime)
    {
        npcController = _npc;
        waitTime = _waitTime;
    }

    public void Execute()
    {
        npcController.agent.isStopped = true;  // NavMeshAgent ����
        npcController.agent.velocity = Vector3.zero;  // �̵� �ӵ� �ʱ�ȭ
        npcController.agent.updatePosition = false;  // NavMesh ��ġ ������Ʈ ����
        npcController.animator.SetTrigger("Look");

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= waitTime)
        {
            finished = true;
        }

    }

    public bool IsFinished()
    {
        return finished;
    }

    public void End()
    {
        Debug.Log($"{npcController.npcName}�� ��ƾ �� ��� ����");
        npcController.agent.isStopped = false;  // NavMeshAgent ����
        npcController.agent.updatePosition = true;
        npcController.animator.ResetTrigger("Look");
    }
}
