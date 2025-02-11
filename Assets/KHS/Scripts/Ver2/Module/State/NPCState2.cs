using System.Collections;
using UnityEngine;


public abstract class NPCState2
{
    protected NPCController _npcController;

    public NPCState2(NPCController npc)
    {
        _npcController = npc;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Execute()
    {
        Debug.Log($"{_npcController.npcName} - Executing Commands in {this.GetType().Name}");
        if (_npcController.GetComponent<NamedController>())
            _npcController.routineInvoker.ExcuteRoutine();
        else
            _npcController.Invoker.ExecuteCommands();
    }
    public virtual IEnumerator JudgeCoroutine(bool _stateCheck, NPCState2 _nextState)
    {
        Execute();
        if (_stateCheck)  // 상태 변경 조건 확인
        {
            _npcController.stateMachine.ChangeState(_nextState);  // 다음 상태로 전환
            yield break;
        }
        yield return null;
    }
    public abstract void Update();
}