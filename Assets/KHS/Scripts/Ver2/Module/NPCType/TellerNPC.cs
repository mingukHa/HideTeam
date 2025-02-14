using UnityEngine;

public class TellerNPC : NPCType
{
    public TellerNPC(TellerController npc) : base(npc)
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
