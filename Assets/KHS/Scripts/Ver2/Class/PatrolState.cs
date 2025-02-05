using UnityEngine;

public class PatrolState : NPCState
{
    public PatrolState(TNPCController npc) : base(npc)
    {
        actions.Add(new PatrolCommand(npc));
    }

    public override void Update()
    {
        // �߰��Ǹ� �߰� ���·� ����
        if (npc.CanPatrol())
        {
            npc.ChangeState(new ChasingState(npc));
        }
    }
}
