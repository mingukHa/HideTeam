using System.Collections.Generic;
using UnityEngine;

public abstract class NPCType : MonoBehaviour
{
    protected NPCController _npcController;


    public NPCType(NPCController npc)
    {
        _npcController = npc;
    }

    public abstract void PerformCommand();

    public abstract bool ChangeStateCondition();
}