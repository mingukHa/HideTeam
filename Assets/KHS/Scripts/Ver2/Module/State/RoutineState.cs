using Unity.AI.Navigation.Samples;
using Unity.VisualScripting;
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
        routineInvoker.ExcuteRoutine();
    }

    public override void Update()
    {
        NPCType2 npcType = _npcController.npcType;

        if (npcType is NamedNPC2)
        {
            NamedController namedController = _npcController.GetComponent<NamedController>();
            Debug.Log("���ӵ� �ڷ�ƾ ���� üũ");
            _npcController.StartCoroutine(JudgeCoroutine(namedController.CurrentRoutineEnd(), new RoutineState(namedController)));
        }
    }
    public override void Exit()
    {
        
    }
}
