using UnityEngine;

public class RunState : IState
{
    private RuntimeAnimatorController animatorController;

    public RunState(RuntimeAnimatorController animator)
    {
        this.animatorController = animator;
    }

    public void EnterState(NPC npc)
    {
        Debug.Log($"{npc.name} ���� ��ȯ: �޸���");
        npc.AssignAnimator(animatorController); // �޸��� �ִϸ����� ����
    }

    public void UpdateState(NPC npc)
    {
        npc.Move(npc.runSpeed); // �޸��� �ӵ��� �̵�
        if (!npc.IsInHurry)
        {
            npc.ChangeState(new WalkState(npc.walkAnimator)); // �ȱ�� ��ȯ
        }
    }

    public void ExitState(NPC npc)
    {
        Debug.Log($"{npc.name} ���� ����: �޸���");
    }
}
