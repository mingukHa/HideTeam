using UnityEngine;

public class RunToCommand : ICommand
{
    private NPCController npcController;
    private Vector3 targetPosition;
    private bool finished = false;

    public RunToCommand(NPCController _npc, Vector3 _targetPosition)
    {
        npcController = _npc;
        targetPosition = _targetPosition;
    }

    public void Execute()
    {
        Debug.Log($"{npcController.npcName} 가 {targetPosition}을 향해 Destination중");
        npcController.animator.SetTrigger("Run");
        npcController.agent.isStopped = false;  // NavMeshAgent 동작
        npcController.agent.updatePosition = true;  // NavMesh 위치 업데이트 시작
        npcController.agent.speed = npcController.runSpeed;
        npcController.agent.SetDestination(targetPosition);
        if (Vector3.Distance(npcController.transform.position, targetPosition) <= npcController.agent.stoppingDistance)
        {
            Debug.Log($"{npcController.npcName} 가 {targetPosition}에 도착 판정");
            finished = true;
        }
    }
    public bool IsFinished() => finished;

    public void End()
    {
        npcController.animator.ResetTrigger("Run");
        npcController.agent.isStopped = true;  // NavMeshAgent 멈춤
        npcController.agent.speed = npcController.walkSpeed;
        npcController.agent.velocity = Vector3.zero;  // 이동 속도 초기화
        npcController.agent.updatePosition = false;  // NavMesh 위치 업데이트 중지
        Debug.Log($"{npcController.npcName}이 {targetPosition}으로 단일 이동 완료");
    }

}
