using UnityEngine;

public abstract class NPCType
{
    protected TNPCController _npcController;

    public NPCType(TNPCController npc)
    {
        _npcController = npc;
    }

    public abstract void PerformCommand();

    public abstract bool ChangeStateCondition();
}
