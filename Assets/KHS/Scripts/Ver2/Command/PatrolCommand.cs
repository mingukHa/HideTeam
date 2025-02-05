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
        Debug.Log("���� ����");
        npc.StartPatrol();
    }
    public void End()
    {
        Debug.Log("���� ����");
        npc.EndPatrol();
    }
}
