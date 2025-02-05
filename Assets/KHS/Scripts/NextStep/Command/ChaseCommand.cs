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
        Debug.Log("추격 시작");
        npc.StartChase();
        npc.MoveToTarget();
    }
    public void End()
    {
        Debug.Log("추격 종료");
        npc.EndChase();
    }
}
