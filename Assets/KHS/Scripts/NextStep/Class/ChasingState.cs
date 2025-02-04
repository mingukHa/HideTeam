using UnityEngine;

public class ChasingState : NPCState
{
    public ChasingState(TNPCController npc) : base(npc)
    {
        actions.Add(new ChaseCommand(npc));
    }

    public override void Update()
    {
        // �÷��̾ ��ġ�� �ٽ� Ž�� ���·� ����
        if (!npc.PlayerVisible())
        {
            npc.ChangeState(new DetectingState(npc));
        }
    }
}
