using UnityEngine;

public abstract class NPCState
{
    protected TNPCController _npcController;

    public NPCState(TNPCController npc)
    {
        _npcController = npc;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public abstract void Update();
}