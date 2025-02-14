using UnityEngine;

public class AngryState : NPCState
{
    private bool isInit = false;
    public AngryState(NPCController npc) : base(npc)
    {
    }

    public override void Enter()
    {
        _npcController.routineInvoker.RoutineChange(1);
    }
    public override void Update()
    {
        if(_npcController.routineInvoker.RoutineEnd() && !isInit)
        {
            isInit = true;
            _npcController.StartCoroutine(_npcController.GetComponent<NamedController>().DialogueCoroutine());
        }
        if(_npcController.GetComponent<NamedController>().DialogueEnd())
        {
            _npcController.stateMachine.ChangeState(new RoutineState(_npcController));
        }
    }
}
