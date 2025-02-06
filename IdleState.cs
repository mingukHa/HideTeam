using UnityEngine;

public class IdleState : NPCState
{
    public IdleState(TNPCController npc) : base(npc) { }

    public override void Enter()
    {
        Debug.Log(_npcController.npcName + " is now IDLE.");
        _npcController.Invoker.AddCommand(new DetectCommand(_npcController));  // Detect target on entering idle state
    }

    public override void Update()
    {
        _npcController.Invoker.ExecuteCommands();
        if (_npcController.HasDetectedTarget())  // Check if target is detected
        {
            _npcController.stateMachine.ChangeState(new ChaseState(_npcController));  // Transition to chase if target is detected
        }
    }
}
