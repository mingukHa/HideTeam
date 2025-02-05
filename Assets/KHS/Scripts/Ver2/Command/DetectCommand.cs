using UnityEngine;

public class DetectCommand : ICommand
{
    private TNPCController npc;

    public DetectCommand(TNPCController npc)
    {
        this.npc = npc;
    }

    public void Execute()
    {
        Debug.Log("Å½Áö ½ÃÀÛ");
        npc.StartDetection();
    }
    public void End()
    {
        Debug.Log("Å½Áö Á¾·á");
        npc.EndDetection();
    }
}