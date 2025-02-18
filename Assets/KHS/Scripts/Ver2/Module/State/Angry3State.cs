using UnityEngine;

public class Angry3State : NPCState
{
    public Angry3State(NPCController npc) : base(npc)
    {

    }

    public override void Enter()
    {
        _npcController.routineInvoker.RoutineChange(4);
    }

    public override void Update()
    {
        if (!_npcController.routineInvoker.RoutineEnd()) // 루틴이 끝나지 않았다면 실행
        {
            Debug.Log("Angry1 State Update 체크");
            _npcController.routineInvoker.ExcuteRoutine();
        }
        else
        {
            _npcController.stateMachine.ChangeState(new GoneState(_npcController));
        }
    }
}
