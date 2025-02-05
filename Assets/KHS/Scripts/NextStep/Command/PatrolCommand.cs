using UnityEngine;
public class PatrolCommand : ICommand
{
    private TNPCController npc;

    public PatrolCommand(TNPCController npc)
    {
        this.npc = npc;
    }

    public void Execute()
    {
        Debug.Log("¼øÂû ½ÃÀÛ");
        npc.StartPatrol();
    }
    public void End()
    {
        Debug.Log("¼øÂû Á¾·á");
        npc.EndPatrol();
    }
}
