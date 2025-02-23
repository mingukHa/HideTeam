using UnityEngine;

public class TalkState : NPCState
{
    public TalkState(NPCController npc) : base(npc)
    {
    }

    public override void Enter()
    {
        _npcController.routineInvoker.RoutineChange(0);
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
            _npcController.stateMachine.ChangeState(new IdleState(_npcController));
        }
    }
}
