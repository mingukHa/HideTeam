using System.Collections.Generic;

public class NPCData
{
    public string npcID;
    public IName nameComponent;
    public IFaction factionComponent;

    public List<AIModule> aiModules;
    public List<Event> events;
}
