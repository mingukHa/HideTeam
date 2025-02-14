using System.Collections.Generic;
using Unity.VisualScripting;

public class InteractState : NPCState
{

    public InteractState(NPCController npc) : base(npc)
    {

    }

    public override void Enter()
    {
        _npcController.animator.SetTrigger("Talk");
        _npcController.GetComponent<TellerController>().StartCoroutine(_npcController.GetComponent<TellerController>().DialogueCoroutine());
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        _npcController.animator.ResetTrigger("Talk");
        _npcController.routineInvoker.RoutineChange(_npcController.GetComponent<TellerController>().interactType);
        _npcController.stateMachine.ChangeState(new RoutineState(_npcController));
    }

}
