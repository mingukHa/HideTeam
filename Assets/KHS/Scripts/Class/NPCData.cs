using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    public string npcID;
    public IName nameComponent;
    public IFaction factionComponent;
    public AIConState currentState = AIConState.Idle;

    public List<AIModule> aiModules;
    public List<Event> events;

    private void Start()
    {
        currentState = AIConState.Idle;
    }
}
