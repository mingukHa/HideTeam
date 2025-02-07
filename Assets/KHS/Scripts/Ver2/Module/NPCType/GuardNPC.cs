using UnityEngine;

public class GuardNPC : NPCType
{
    public GuardNPC(GuardController npc) : base(npc)
    {
        
    }

    public override void PerformCommand()
    {

    }

    public override bool ChangeStateCondition()
    {
        // 예: 특정 조건에서만 상태를 변경 (예: 경계 구역에 접근한 경우)
        return _npcController.HasDetectedTarget();
    }
}
