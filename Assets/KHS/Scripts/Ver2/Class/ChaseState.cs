using UnityEngine;

public class ChaseState : NPCState
{
    public ChaseState(TNPCController npc) : base(npc) { }

    public override void Enter()
    {
        Debug.Log(_npcController.npcName + " is now CHASING.");
        _npcController.Invoker.AddCommand(new MoveCommand(_npcController, _npcController.GetTargetPosition()));
    }

    public override void Update()
    {
        _npcController.Invoker.ExecuteCommands();
        if (_npcController.HasDetectedTarget() == false)
        {
            _npcController.stateMachine.ChangeState(new IdleState(_npcController));
        }
    }
}
