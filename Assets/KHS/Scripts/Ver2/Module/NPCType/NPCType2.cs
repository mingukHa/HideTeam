using System.Collections.Generic;
using UnityEngine;

public abstract class NPCType2 : MonoBehaviour
{
    protected NPCController _npcController;
    public List<ICommand> _commandList;

    public NPCType2(NPCController npc)
    {
        _npcController = npc;
        _commandList = new List<ICommand>();
    }

    public abstract void PerformCommand();

    public abstract bool ChangeStateCondition();
}