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
        Debug.Log($"{npc.name} 상태 전환: 걷기");
        npc.AssignAnimator(animatorController); // 걷기 애니메이터 설정
    }

    public void UpdateState(NPC npc)
    {
        npc.Move(npc.walkSpeed); // 걷기 속도로 이동
        if (npc.IsInHurry)
        {
            npc.ChangeState(new RunState(npc.runAnimator)); // 달리기로 전환
        }
    }

    public void ExitState(NPC npc)
    {
        Debug.Log($"{npc.name} 상태 종료: 걷기");
    }
}
