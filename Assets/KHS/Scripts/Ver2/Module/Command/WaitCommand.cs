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
        npcController.agent.isStopped = true;  // NavMeshAgent 멈춤
        npcController.agent.velocity = Vector3.zero;  // 이동 속도 초기화
        npcController.agent.updatePosition = false;  // NavMesh 위치 업데이트 중지
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
        Debug.Log($"{npcController.npcName}의 루틴 중 대기 종료");
        npcController.agent.isStopped = false;  // NavMeshAgent 멈춤
        npcController.agent.updatePosition = true;
        npcController.animator.ResetTrigger("Look");
    }
}
