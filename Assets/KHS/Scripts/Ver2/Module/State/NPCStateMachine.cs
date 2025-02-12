using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
    public NPCState _currentState;
    private float elapsedTime = 0;
    public void ChangeState(NPCState newState)
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
        {
            Debug.Log(_currentState);
            _currentState.Update();
        }
    }
}
