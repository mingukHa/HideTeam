using UnityEngine;

public class LookAroundState : IState
{
    private RuntimeAnimatorController animatorController;

    public LookAroundState(RuntimeAnimatorController animator)
    {
        this.animatorController = animator;
    }

    public void EnterState(NPC npc)
    {
        Debug.Log($"{npc.name} ���� ��ȯ: �θ����Ÿ���");
        npc.AssignAnimator(animatorController); // �θ����Ÿ��� �ִϸ����� ����
    }

    public void UpdateState(NPC npc)
    {
        if (npc.ShouldStartWalking)
        {
            npc.ChangeState(new WalkState(npc.walkAnimator)); // �ȱ� ���·� ��ȯ
        }
    }

    public void ExitState(NPC npc)
    {
        Debug.Log($"{npc.name} ���� ����: �θ����Ÿ���");
    }
}
