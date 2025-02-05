using UnityEngine;

public class StateController : MonoBehaviour
{
    private NPCState currentState;

    public void ChangeState(NPCState newState)
    {
        if (currentState != null)
            currentState.Exit(); // 이전 상태 정리

        currentState = newState;
        currentState.Enter(); // 새로운 상태 실행
    }

    public void UpdateState()
    {
        if (currentState != null)
            currentState.Update();
    }
}
