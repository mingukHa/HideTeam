using UnityEngine;

public class TotalEndState : NPCState
{
    public TotalEndState(NPCController npc) : base(npc)
    {
    }

    public override void Enter()
    {
        _npcController.routineInvoker.RoutineChange(6);
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