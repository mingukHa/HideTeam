using UnityEngine;

public class WaitState : NPCState2
{
    private RoutineInvoker routineInvoker;

    public WaitState(NPCController npc) : base(npc)
    {
        routineInvoker = npc.GetComponent<RoutineInvoker>();
    }

    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}�� ��� ���¿� ����");

    }

    public override void Update()
    {

    }
}
