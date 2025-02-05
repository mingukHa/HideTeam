using System.Collections.Generic;
using UnityEngine;

public class ENPCController : MonoBehaviour
{
    protected int npcID;
    protected EFaction npcFaction = EFaction.NULL;

    public ENPCController(int _npcID, EFaction _npcFaction)
    {
        this.npcID = _npcID;
        this.npcFaction = _npcFaction;
    }


    public List<ENPCClassType>npcClassType;
}
