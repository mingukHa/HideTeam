using UnityEngine;

public class RoutineState : NPCState2
{
    private RoutineInvoker routineInvoker;

    public RoutineState(NPCController npc) : base(npc)
    {
        routineInvoker = npc.GetComponent<RoutineInvoker>();
    }

    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}이 루틴 상태에 진입");
        
    }

    public override void Update()
    {
        if (!routineInvoker.RoutineEnd()) // 루틴이 끝나지 않았다면 실행
        {
            Debug.Log("루틴 스테이트 업데이트 체크");
            routineInvoker.ExcuteRoutine();
        }
        else
        {
            _npcController.stateMachine.ChangeState(new WaitState(_npcController));
        }
    }
}
