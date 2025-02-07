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
        if (npcType.GetComponent<DefaultNPC>())
        {
            Debug.Log(_npcController.npcName + " is now IDLE.");
            _npcController.Invoker.AddCommand(new DetectCommand(_npcController));  // Idle 상태에서 타겟 감지 명령 추가
        }
        if (npcType.GetComponent<GuardNPC>())
        {
            Debug.Log(_npcController.npcName + " is now IDLE.");
            GuardController _guardController = _npcController.GetComponent<GuardController>();
            _npcController.Invoker.AddCommand(new PatrolCommand(_guardController));
            _npcController.Invoker.AddCommand(new DetectCommand(_guardController));
        }
        if (npcType.GetComponent<NamedNPC>())
        {
            Debug.Log(_npcController.npcName + " is now IDLE.");
            NamedNPCController _namedNPCController = _npcController.GetComponent<NamedNPCController>();
            _npcController.Invoker.AddCommand(new RoutineCommand(_namedNPCController));
        }
    }
    public override void Update()
    {
        
        if (_npcController.GetComponent<NamedNPC>())
        {
            Debug.Log("네임드 코루틴 진입 체크");
            _npcController.StartCoroutine(JudgeCoroutine(_npcController.GetComponent<NamedNPCController>().Routine(), new WaitState(_npcController)));
        }
        if(_npcController.GetComponent<GuardNPC>() || _npcController.GetComponent<DefaultNPC>())
        {
            Debug.Log("네임드 외 코루틴 진입 체크");
            _npcController.StartCoroutine(JudgeCoroutine(_npcController.HasDetectedTarget(), new ChaseState(_npcController)));
        }
    }
}