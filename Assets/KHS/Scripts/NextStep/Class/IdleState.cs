using UnityEngine;

public class IdleState : NPCState
{
    public IdleState(TNPCController npc) : base(npc) { }

    public override void Update()
    {
        // Ž�� ������ �Ǹ� DetectingState�� ����
        if (npc.CanDetectPlayer())
        {
            npc.ChangeState(new DetectingState(npc));
        }
    }
}
