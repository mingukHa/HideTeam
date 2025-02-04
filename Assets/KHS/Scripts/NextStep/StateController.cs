using UnityEngine;

public class StateController : MonoBehaviour
{
    private NPCState currentState;

    public void ChangeState(NPCState newState)
    {
        if (currentState != null)
            currentState.Exit(); // ���� ���� ����

        currentState = newState;
        currentState.Enter(); // ���ο� ���� ����
    }

    public void UpdateState()
    {
        if (currentState != null)
            currentState.Update();
    }
}
