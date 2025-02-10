using UnityEngine;

public class IdleState2 : NPCState2
{

    public IdleState2(NPCController npc) : base(npc) 
    {

    }

    public override void Enter()
    {
        NPCType2 npcType = _npcController.npcType;
        Debug.Log($"{_npcController.npcName} - IdleState2 Enter() ȣ���! NPCType: {npcType.GetType()}");

        if (npcType is NamedNPC2)
        {
            Debug.Log(_npcController.npcName + " is now IDLE2 as NamedNPC2.");
            NamedNPCController2 _namedNPCController = _npcController.GetComponent<NamedNPCController2>();
            Debug.Log("Adding RoutineCommand2 to Invoker.");
            _npcController.Invoker.AddCommand(new RoutineCommand2(_namedNPCController));
            _npcController.Invoker.AddCommand(new MoveToNavMeshCommand(_namedNPCController));  // �̵� ��� �߰�
            //_npcController.Invoker.AddCommand(new MoveToNavMeshCommand(_namedNPCController));
        }
    }
    public override void Update()
    {
        NPCType2 npcType = _npcController.npcType;

        if (npcType is NamedNPC2)
        {
            NamedNPCController2 namedNPCController = _npcController.GetComponent<NamedNPCController2>();
            Debug.Log("���ӵ� �ڷ�ƾ ���� üũ");
            _npcController.StartCoroutine(JudgeCoroutine(namedNPCController.EventTrigger(0), new WaitState2(namedNPCController)));
        }
        else
        {
            Debug.Log("���ӵ� �� �ڷ�ƾ ���� üũ");
            
        }
    }
}