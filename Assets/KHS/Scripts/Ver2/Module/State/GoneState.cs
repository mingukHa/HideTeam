using UnityEngine;

public class GoneState : NPCState
{
    public GoneState(NPCController npc) : base(npc)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}�� GoneState�� ��ȯ");
        _npcController.routineInvoker.RoutineChange(1);
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
