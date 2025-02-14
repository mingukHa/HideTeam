using System.Collections;
using UnityEngine;

public class PatrolState : NPCState
{
    private float patrolTime = 30f;
    private float elapsedTime = 0f;

    public PatrolState(NPCController npc) : base(npc) { }

    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}�� 3�� ���� ������ ����.");
        _npcController.routineInvoker.RoutineChange(2);
    }

    public override void Update()
    {
        elapsedTime += Time.deltaTime;
        Debug.Log($"{elapsedTime} : {patrolTime} = {CheckTime()}");
        if (!_npcController.routineInvoker.RoutineEnd() && !CheckTime()) // ��ƾ�� ������ �ʾҴٸ� ����
        {
            Debug.Log("Patrol State Update üũ");
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
