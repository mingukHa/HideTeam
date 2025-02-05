using UnityEngine;

public class PatrolState : NPCState
{
    public PatrolState(TNPCController npc) : base(npc)
    {
        actions.Add(new PatrolCommand(npc));
    }

    public override void Update()
    {
        // 발각되면 추격 상태로 변경
        if (npc.CanPatrol())
        {
            npc.ChangeState(new ChasingState(npc));
        }
    }
}
