using UnityEngine;

public class RoutineState : NPCState
{
    public RoutineState(NPCController npc) : base(npc)
    {
        
    }

    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}�� ��ƾ ���¿� ����");
        _npcController.routineInvoker.RoutineChange(0);
    }

    public override void Update()
    {
        
        if (!_npcController.routineInvoker.RoutineEnd()) // ��ƾ�� ������ �ʾҴٸ� ����
        {
            Debug.Log("Routine State Update üũ");
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
