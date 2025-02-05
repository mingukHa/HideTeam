using UnityEngine;

public class IdleState : NPCState
{
    public IdleState(TNPCController npc) : base(npc) { }

    public override void Update()
    {
        // 탐지 조건이 되면 DetectingState로 변경
        if (npc.CanDetectPlayer())
        {
            npc.ChangeState(new DetectingState(npc));
        }
    }
}
