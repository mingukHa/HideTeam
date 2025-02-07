using System.Collections.Generic;
using UnityEngine;

public abstract class NPCType : MonoBehaviour
{
    protected TNPCController _npcController;
    public List<ICommand> _commandList;

    public NPCType(TNPCController npc)
    {
        _npcController = npc;
        _commandList = new List<ICommand>();
    }

    public abstract void PerformCommand();

    public abstract bool ChangeStateCondition();
}
