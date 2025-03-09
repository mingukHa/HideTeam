using UnityEngine;

public class GoneState : NPCState
{
    public GoneState(NPCController npc) : base(npc)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}이 GoneState로 전환");
        _npcController.routineInvoker.RoutineChange(4);
    }
    public override void Update()
    {
        if (!_npcController.routineInvoker.RoutineEnd()) // 루틴이 끝나지 않았다면 실행
        {
            Debug.Log("Gone State Update 체크");
            _npcController.routineInvoker.ExcuteRoutine();
        }
        else
        {
            _npcController.animator.ResetTrigger("Walk");
            _npcController.stateMachine.ChangeState(new IdleState(_npcController));
        }
    }

}
