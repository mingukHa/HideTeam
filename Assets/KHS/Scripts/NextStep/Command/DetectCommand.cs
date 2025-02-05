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
        Debug.Log("Ž�� ����");
        npc.StartDetection();
    }
    public void End()
    {
        Debug.Log("Ž�� ����");
        npc.EndDetection();
    }
}