using UnityEngine;

public class StateController : MonoBehaviour
{
    private NPCState currentState;

    public void ChangeState(NPCState _State)
    {
        if (currentState != null)
            currentState.Exit(); // 이전 상태 정리

        currentState = _State;
        currentState.Enter(); // 새로운 상태 실행
    }

    public void UpdateState()
    {
        if (currentState != null)
            currentState.Update();
    }
}
