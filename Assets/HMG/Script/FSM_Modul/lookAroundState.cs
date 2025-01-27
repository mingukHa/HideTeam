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
        Debug.Log($"{npc.name} 상태 전환: 두리번거리기");
        npc.AssignAnimator(animatorController); // 두리번거리기 애니메이터 설정
    }

    public void UpdateState(NPC npc)
    {
        if (npc.ShouldStartWalking)
        {
            npc.ChangeState(new WalkState(npc.walkAnimator)); // 걷기 상태로 전환
        }
    }

    public void ExitState(NPC npc)
    {
        Debug.Log($"{npc.name} 상태 종료: 두리번거리기");
    }
}
