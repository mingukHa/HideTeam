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
        Debug.Log($"{_npcController.npcName} - IdleState Enter() 호출됨! NPCType: {npcType.GetType()}");

        if (npcType is GuardNPC)
        {
            Debug.Log(_npcController.npcName + " is now IDLE as GuardNPC.");
            GuardController _guardController = _npcController.GetComponent<GuardController>();
            _npcController.Invoker.AddCommand(new PatrolCommand(_guardController));
            _npcController.Invoker.AddCommand(new DetectCommand(_guardController));
        }

        if (npcType is NamedNPC)
        {
            Debug.Log(_npcController.npcName + " is now IDLE as NamedNPC.");
            NamedNPCController _namedNPCController = _npcController.GetComponent<NamedNPCController>();
            Debug.Log("Adding RoutineCommand to Invoker.");
            _npcController.Invoker.AddCommand(new RoutineCommand(_namedNPCController));
        }
        if (npcType is DefaultNPC)
        {
            Debug.Log(_npcController.npcName + " is now IDLE as DefaultNPC.");
            _npcController.Invoker.AddCommand(new DetectCommand(_npcController));  // Idle 상태에서 타겟 감지 명령 추가
        }
    }
    public override void Update()
    {
        NPCType npcType = _npcController.npcType;

        if (npcType is NamedNPC)
        {
            Debug.Log("네임드 코루틴 진입 체크");
            Debug.Log($"{_npcController.npcName} - Checking Routine Condition - " + _npcController.GetComponent<NamedNPCController>().RoutineCheck());
            _npcController.StartCoroutine(JudgeCoroutine(_npcController.GetComponent<NamedNPCController>().RoutineCheck(), new WaitState(_npcController)));
        }
        else
        {
            Debug.Log("네임드 외 코루틴 진입 체크");
            _npcController.StartCoroutine(JudgeCoroutine(_npcController.HasDetectedTarget(), new ChaseState(_npcController)));
        }
    }
}