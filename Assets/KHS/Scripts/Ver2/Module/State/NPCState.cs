using System.Collections;
using UnityEngine;


public abstract class NPCState
{
    protected NPCController _npcController;

    public NPCState(NPCController npc)
    {
        _npcController = npc;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Execute()
    {
        Debug.Log($"{_npcController.npcName} - Executing Commands in {this.GetType().Name}");
        _npcController.routineInvoker.ExcuteRoutine();
    }
    public abstract void Update();
}