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
        Debug.Log($"{_npcController.npcName}�� ��ƾ ���¿� ����");
        
    }

    public override void Update()
    {
        if (!routineInvoker.RoutineEnd()) // ��ƾ�� ������ �ʾҴٸ� ����
        {
            Debug.Log("��ƾ ������Ʈ ������Ʈ üũ");
            routineInvoker.ExcuteRoutine();
        }
        else
        {
            _npcController.stateMachine.ChangeState(new WaitState(_npcController));
        }
    }
}
