using UnityEngine;

public class NPCStateMachine2 : MonoBehaviour
{
    private NPCState2 _currentState;

    public void ChangeState(NPCState2 newState)
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = newState;

        if (_currentState != null)
            _currentState.Enter();
    }

    public void Update()
    {
        if (_currentState != null)
            Debug.Log(_currentState);
            _currentState.Update();
    }
}
