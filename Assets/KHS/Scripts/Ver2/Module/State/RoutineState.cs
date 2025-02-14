using UnityEngine;

public class RoutineState : NPCState
{
    public RoutineState(NPCController npc) : base(npc)
    {
        
    }

    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}이 루틴 상태에 진입");
        _npcController.routineInvoker.RoutineChange(0);
    }

    public override void Update()
    {
        
        if (!_npcController.routineInvoker.RoutineEnd()) // 루틴이 끝나지 않았다면 실행
        {
            Debug.Log("Routine State Update 체크");
            _npcController.routineInvoker.ExcuteRoutine();
        }
        else
        {
            _npcController.stateMachine.ChangeState(new RoutineState(_npcController));
        }
    }
    public override void Exit()
    {

    }
}
