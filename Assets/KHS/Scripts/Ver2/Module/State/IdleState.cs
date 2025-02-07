using UnityEngine;

public class IdleState : NPCState
{
    public IdleState(TNPCController npc) : base(npc) { }

    public override void Enter()
    {
        Debug.Log(_npcController.npcName + " is now IDLE.");
        _npcController.Invoker.AddCommand(new DetectCommand(_npcController));  // Idle 상태에서 타겟 감지 명령 추가
    }

    public override void Update()
    {
        _npcController.Invoker.ExecuteCommands();
        if (_npcController.HasDetectedTarget())  // 타겟이 감지되었으면
        {
            _npcController.stateMachine.ChangeState(new ChaseState(_npcController));  // Chase 상태로 전환
        }
    }
}