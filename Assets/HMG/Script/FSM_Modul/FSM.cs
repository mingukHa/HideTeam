using Unity.VisualScripting;
using UnityEngine;

public class FSM //상태 변경 스크립트
{
    private IState currentState;

    public void ChangeState(IState newState, NPC npc)
    {
        if (newState == null)
        {
            Debug.LogError("새 상태가 null입니다. 상태를 확인하세요.");
            return;
        }

        currentState?.ExitState(npc); // 이전 상태 종료
        currentState = newState;
        currentState.EnterState(npc); // 새 상태 시작
    }

    public void Update(NPC npc)
    {
        currentState?.UpdateState(npc);
    }
}
