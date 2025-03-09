using UnityEngine;

public class IdleState : NPCState
{
    public IdleState(NPCController npc) : base(npc)
    {

    }

    public override void Enter()
    {
        _npcController.animator.SetTrigger("Idle");
    }

    public override void Update()
    {
        
    }
}
