using UnityEngine;

public class WalkState : IState
{
    private RuntimeAnimatorController animatorController;

    public WalkState(RuntimeAnimatorController animator)
    {
        this.animatorController = animator;
    }

    public void EnterState(NomalNPC npc)
    {
        Debug.Log($"{npc.name} ���� ��ȯ: �ȱ�");
        npc.AssignAnimator(animatorController); // �ȱ� �ִϸ����� ����
    }

    public void UpdateState(NomalNPC npc)
    {
        npc.Move(npc.walkSpeed); // �ȱ� �ӵ��� �̵�
        if (npc.IsInHurry)
        {
            npc.ChangeState(new RunState(npc.runAnimator)); // �޸���� ��ȯ
        }
    }

    public void ExitState(NomalNPC npc)
    {
        Debug.Log($"{npc.name} ���� ����: �ȱ�");
    }
}
