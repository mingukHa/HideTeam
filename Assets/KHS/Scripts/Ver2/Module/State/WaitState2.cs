using UnityEngine;

public class WaitState2 : NPCState2
{
    public WaitState2(NPCController npc) : base(npc)
    {

    }
    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName} - WaitState2 Enter() ȣ���!");

        Debug.Log(_npcController.npcName + " is now WAIT2 as NamedNPC2.");
        NamedNPCController2 _namedNPCController = _npcController.GetComponent<NamedNPCController2>();
        Debug.Log("Adding RoutineCommand2 to Invoker.");
        _npcController.Invoker.AddCommand(new RoutineChangeCommand2(_namedNPCController));
    }

    public override void Update()
    {
        NPCType2 npcType = _npcController.npcType;

        if (npcType is NamedNPC2)
        {
            NamedNPCController2 namedNPCController = _npcController.GetComponent<NamedNPCController2>();
            Debug.Log("���ӵ� �ڷ�ƾ ���� üũ");
            _npcController.StartCoroutine(JudgeCoroutine(!namedNPCController.EventTrigger(0), new IdleState2(namedNPCController)));
        }
    }
}
