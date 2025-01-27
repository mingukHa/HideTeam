using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "Game/NPC")]
public class NPCData : ScriptableObject
{
    [Header("Basic Info")]
    public string npcName;
    public Faction faction;

    [Header("AI Module")]
    public List<AIModule> aiModules;

    [Header("Event Triggers")]
    public List<EventTrigger> eventTriggers;
}
