using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class Angry2State : NPCState
{
    public Angry2State(NPCController npc) : base(npc)
    {
    }

    public override void Enter()
    {
        _npcController.routineInvoker.RoutineChange(2);
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
            _npcController.stateMachine.ChangeState(new Angry3State(_npcController));
        }
    }
}