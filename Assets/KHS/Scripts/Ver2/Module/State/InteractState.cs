using UnityEngine;

public class InteractState : NPCState
{

    public InteractState(NPCController npc) : base(npc)
    {

    }

    public override void Enter()
    {
        _npcController.animator.SetTrigger("Talk");
    }

    public override void Update()
    {

        if (!_npcController.routineInvoker.RoutineEnd()) // 루틴이 끝나지 않았다면 실행
        {
            Debug.Log("Interact State Update 체크");
            _npcController.routineInvoker.ExcuteRoutine();
        }
        else
        {
            _npcController.stateMachine.ChangeState(new InteractState(_npcController));
        }
    }

    public override void Exit()
    {
        _npcController.animator.ResetTrigger("Talk");
        _npcController.stateMachine.ChangeState(new RoutineState(_npcController));
    }

}
