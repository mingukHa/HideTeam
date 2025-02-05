using UnityEngine;

public class DetectingState : NPCState
{
    public DetectingState(TNPCController npc) : base(npc)
    {
        actions.Add(new DetectCommand(npc));
    }

    public override void Update()
    {
        // �߰��Ǹ� �߰� ���·� ����
        if (npc.PlayerSpotted())
        {
            npc.ChangeState(new ChasingState(npc));
        }
    }
}
