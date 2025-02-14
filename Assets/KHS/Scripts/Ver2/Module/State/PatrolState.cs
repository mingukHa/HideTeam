using System.Collections;
using UnityEngine;

public class PatrolState : NPCState
{
    private float patrolTime = 30f;
    private float elapsedTime = 0f;

    public PatrolState(NPCController npc) : base(npc) { }

    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}가 3분 동안 순찰을 시작.");
        _npcController.routineInvoker.RoutineChange(2);
    }

    public override void Update()
    {
        elapsedTime += Time.deltaTime;
        Debug.Log($"{elapsedTime} : {patrolTime} = {CheckTime()}");
        if (!_npcController.routineInvoker.RoutineEnd() && !CheckTime()) // 루틴이 끝나지 않았다면 실행
        {
            Debug.Log("Patrol State Update 체크");
            _npcController.routineInvoker.ExcuteRoutine();
        }
        else if(_npcController.routineInvoker.RoutineEnd() && !CheckTime())
        {
            _npcController.routineInvoker.RoutineChange(2);
        }
        else
        {
            _npcController.stateMachine.ChangeState(new RoutineState(_npcController));
        }
    }
    private bool CheckTime()
    {
        if (elapsedTime < patrolTime)
            return false;
        else
            return true;
    }
}
