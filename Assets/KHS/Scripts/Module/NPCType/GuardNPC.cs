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
        // ��: Ư�� ���ǿ����� ���¸� ���� (��: ��� ������ ������ ���)
        return _npcController.HasDetectedTarget();
    }
}
