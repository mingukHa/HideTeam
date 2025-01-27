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
        Debug.Log($"{npc.name} 상태 전환: 달리기");
        npc.AssignAnimator(animatorController); // 달리기 애니메이터 설정
    }

    public void UpdateState(NPC npc)
    {
        npc.Move(npc.runSpeed); // 달리기 속도로 이동
        if (!npc.IsInHurry)
        {
            npc.ChangeState(new WalkState(npc.walkAnimator)); // 걷기로 전환
        }
    }

    public void ExitState(NPC npc)
    {
        Debug.Log($"{npc.name} 상태 종료: 달리기");
    }
}
