using UnityEngine;

public class CallState : NPCState
{
    public CallState(NPCController npc) : base(npc)
    {
    }

    public override void Enter()
    {
        _npcController.routineInvoker.RoutineChange(5);
    }
    public override void Update()
    {
        if (!_npcController.routineInvoker.RoutineEnd()) // ��ƾ�� ������ �ʾҴٸ� ����
        {
            Debug.Log("Gone State Update üũ");
            _npcController.routineInvoker.ExcuteRoutine();
        }
        else
        {
            _npcController.stateMachine.ChangeState(new TotalEndState(_npcController));
        }
    }
}
