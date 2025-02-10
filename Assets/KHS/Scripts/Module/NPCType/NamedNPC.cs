using UnityEngine;

public class NamedNPC : NPCType
{
    public NamedNPC(NamedNPCController npc) : base(npc)
    {

    }
    public override bool ChangeStateCondition()
    {
        return true;
    }

    public override void PerformCommand()
    {

    }
}