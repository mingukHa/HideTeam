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
        npcController.agent.speed = npcController.runSpeed;
        npcController.agent.angularSpeed = 200f;
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
        Debug.Log($"{npcController.npcName}이 {targetPosition}으로 단일 이동 완료");
    }

}
