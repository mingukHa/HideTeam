using UnityEngine;

public class NamedNPC2 : NPCType2
{
    public NamedNPC2(NamedNPCController2 npc) : base(npc)
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
