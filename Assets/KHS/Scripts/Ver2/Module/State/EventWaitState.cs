using System.Collections;
using UnityEngine;

public class EventWaitState : NPCState
{
    public EventWaitState(NPCController npc) : base(npc)
    {
        
    }
    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}�� �̺�Ʈ�� �����Ͽ� 3�� ��� ����");
        _npcController.routineInvoker.RoutineChange(1);
        _npcController.routineInvoker.ExcuteRoutine();
    }

    public override void Update()
    {
        if (!_npcController.routineInvoker.RoutineEnd()) // ��ƾ�� ������ �ʾҴٸ� ����
        {
            Debug.Log("EventWaitState Update üũ");
            _npcController.routineInvoker.ExcuteRoutine();
        }
        else
        {
            _npcController.stateMachine.ChangeState(new PatrolState(_npcController));
        }
    }
}
