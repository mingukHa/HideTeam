using UnityEngine;

public class IdelState : NPCState
{
    public IdelState(NPCController npc) : base(npc)
    {

    }

    public override void Enter()
    {
        _npcController.animator.SetTrigger("Idel");
    }

    public override void Update()
    {
        
    }
}
