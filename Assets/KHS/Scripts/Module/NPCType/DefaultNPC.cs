using UnityEngine;

public class DefaultNPC : NPCType
{
    public DefaultNPC(TNPCController npc) : base(npc)
    {

    }

    public override bool ChangeStateCondition()
    {
        return _npcController.HasDetectedTarget();
    }

    public override void PerformCommand()
    {

    }
}
