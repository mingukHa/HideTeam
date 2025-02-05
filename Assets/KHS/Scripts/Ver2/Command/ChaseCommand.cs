using UnityEngine;
public class ChaseCommand : ICommand
{
    private TNPCController npc;

    public ChaseCommand(TNPCController npc)
    {
        this.npc = npc;
    }

    public void Execute()
    {
        Debug.Log("�߰� ����");
        npc.StartChase();
        npc.MoveToTarget();
    }
    public void End()
    {
        Debug.Log("�߰� ����");
        npc.EndChase();
    }
}
