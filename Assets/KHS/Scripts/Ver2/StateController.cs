using UnityEngine;

public class StateController : MonoBehaviour
{
    private NPCState currentState;

    public void ChangeState(NPCState _State)
    {
        if (currentState != null)
            currentState.Exit(); // ���� ���� ����

        currentState = _State;
        currentState.Enter(); // ���ο� ���� ����
    }

    public void UpdateState()
    {
        if (currentState != null)
            currentState.Update();
    }
}
