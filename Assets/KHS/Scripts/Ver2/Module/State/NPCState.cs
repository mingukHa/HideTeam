using System.Collections;
using UnityEngine;


public abstract class NPCState
{
    protected NPCController _npcController;

    public NPCState(NPCController npc)
    {
        _npcController = npc;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Execute()
    {
        Debug.Log($"{_npcController.npcName} - Executing Commands in {this.GetType().Name}");
        _npcController.routineInvoker.ExcuteRoutine();
    }
    public virtual IEnumerator JudgeCoroutine(bool _stateCheck, NPCState _nextState)
    {
        if (_stateCheck)  // ���� ���� ���� Ȯ��
        {
            _npcController.stateMachine.ChangeState(_nextState);  // ���� ���·� ��ȯ
            yield break;
        }
        yield return null;
    }
    public abstract void Update();
}