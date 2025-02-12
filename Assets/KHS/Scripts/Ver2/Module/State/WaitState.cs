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
        Debug.Log($"{_npcController.npcName}이 대기 상태에 진입");
        _npcController.agent.isStopped = true;  // NavMeshAgent 멈춤
        _npcController.agent.velocity = Vector3.zero;  // 이동 속도 초기화
        _npcController.agent.updatePosition = false;  // NavMesh 위치 업데이트 중지
        _npcController.agent.updateRotation = false;  // NavMesh 회전 중지
        _npcController.animator.SetTrigger("Talk");
    }

    public override void Update()
    {

    }
    public override void Exit()
    {
        Debug.Log($"{_npcController.npcName}이 대기 상태에서 탈출");
        _npcController.animator.ResetTrigger("Talk");
    }
}
