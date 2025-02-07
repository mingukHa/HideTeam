using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class IdleState : NPCState
{
    public IdleState(TNPCController npc) : base(npc) 
    {

    }

    public override void Enter()
    {
        NPCType npcType = _npcController.npcType;
        Debug.Log($"{_npcController.npcName} - IdleState Enter() ȣ���! NPCType: {npcType.GetType()}");

        if (npcType is NamedNPC)
        {
            Debug.Log(_npcController.npcName + " is now IDLE as NamedNPC.");
            GuardController _guardController = _npcController.GetComponent<GuardController>();
            _npcController.Invoker.AddCommand(new PatrolCommand(_guardController));
            _npcController.Invoker.AddCommand(new DetectCommand(_guardController));
        }

        if (npcType is GuardNPC)
        {
            Debug.Log(_npcController.npcName + " is now IDLE as GuardNPC.");
            NamedNPCController _namedNPCController = _npcController.GetComponent<NamedNPCController>();
            _npcController.Invoker.AddCommand(new RoutineCommand(_namedNPCController));
        }
        if (npcType is DefaultNPC)
        {
            Debug.Log(_npcController.npcName + " is now IDLE as DefaultNPC.");
            _npcController.Invoker.AddCommand(new DetectCommand(_npcController));  // Idle ���¿��� Ÿ�� ���� ��� �߰�
        }
    }
    public override void Update()
    {
        if (_npcController.GetComponent<NamedNPC>())
        {
            Debug.Log("���ӵ� �ڷ�ƾ ���� üũ");
            _npcController.StartCoroutine(JudgeCoroutine(_npcController.GetComponent<NamedNPCController>().Routine(), new WaitState(_npcController)));
        }

        Debug.Log("���ӵ� �� �ڷ�ƾ ���� üũ");
        _npcController.StartCoroutine(JudgeCoroutine(_npcController.HasDetectedTarget(), new ChaseState(_npcController)));

    }
}