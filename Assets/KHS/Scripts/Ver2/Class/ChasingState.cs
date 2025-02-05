using UnityEngine;

public class ChasingState : NPCState
{
    public ChasingState(TNPCController npc) : base(npc)
    {
        actions.Add(new ChaseCommand(npc));
    }

    public override void Update()
    {
        // 플레이어를 놓치면 다시 탐지 상태로 변경
        if (!npc.PlayerVisible())
        {
            npc.ChangeState(new DetectingState(npc));
        }
    }
}
