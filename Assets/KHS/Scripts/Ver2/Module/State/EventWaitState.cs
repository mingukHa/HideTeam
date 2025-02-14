using System.Collections;
using UnityEngine;

public class EventWaitState : NPCState
{
    public EventWaitState(NPCController npc) : base(npc)
    {
        
    }
    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}이 이벤트를 감지하여 3초 대기 시작");
        _npcController.routineInvoker.RoutineChange(1);
        _npcController.routineInvoker.ExcuteRoutine();
    }

    public override void Update()
    {
        if (!_npcController.routineInvoker.RoutineEnd()) // 루틴이 끝나지 않았다면 실행
        {
            Debug.Log("EventWaitState Update 체크");
            _npcController.routineInvoker.ExcuteRoutine();
        }
        else
        {
            _npcController.stateMachine.ChangeState(new PatrolState(_npcController));
        }
    }
}
