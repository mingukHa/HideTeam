using UnityEngine;

public class DetectingState : NPCState
{
    public DetectingState(TNPCController npc) : base(npc)
    {
        actions.Add(new DetectCommand(npc));
    }

    public override void Update()
    {
        // 발각되면 추격 상태로 변경
        if (npc.PlayerSpotted())
        {
            npc.ChangeState(new ChasingState(npc));
        }
    }
}
