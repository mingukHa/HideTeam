using UnityEngine;

public class TellerGoneState : NPCState
{
    public TellerGoneState(NPCController npc) : base(npc)
    {
    }

    public override void Enter()
    {
        _npcController.routineInvoker.RoutineChange(1);
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
            _npcController.stateMachine.ChangeState(new IdelState(_npcController));
        }
    }


}
