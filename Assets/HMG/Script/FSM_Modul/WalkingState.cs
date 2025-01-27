using UnityEngine;

public class WalkState : IState
{
    private RuntimeAnimatorController animatorController;

    public WalkState(RuntimeAnimatorController animator)
    {
        this.animatorController = animator;
    }

    public void EnterState(NPC npc)
    {
        Debug.Log($"{npc.name} ���� ��ȯ: �ȱ�");
        npc.AssignAnimator(animatorController); // �ȱ� �ִϸ����� ����
    }

    public void UpdateState(NPC npc)
    {
        npc.Move(npc.walkSpeed); // �ȱ� �ӵ��� �̵�
        if (npc.IsInHurry)
        {
            npc.ChangeState(new RunState(npc.runAnimator)); // �޸���� ��ȯ
        }
    }

    public void ExitState(NPC npc)
    {
        Debug.Log($"{npc.name} ���� ����: �ȱ�");
    }
}
