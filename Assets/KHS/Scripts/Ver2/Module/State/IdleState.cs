using UnityEngine;
using System.Collections;

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
    }
    public override void Update()
    {
        _npcController.StartCoroutine(JudgeCoroutine(_npcController.HasDetectedTarget(), new ChaseState(_npcController)));

    }
}