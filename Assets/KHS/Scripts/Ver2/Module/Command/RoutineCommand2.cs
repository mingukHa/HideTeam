using UnityEngine;

public class RoutineCommand2 : ICommand
{
    private NamedNPCController2 _npcController;
    private bool _isFinished;

    public RoutineCommand2(NamedNPCController2 npc)
    {
        this._npcController = npc;
        this._isFinished = false;
    }

    public void Execute()
    {
        Debug.Log("RoutineCommand2 : Executed Call");
        if (_npcController.curRoutineIdx != 0)
            _npcController.ChangeRoutine();

        //_npcController.Invoker.AddCommand(new MoveToNavMeshCommand(_npcController));  // 이동 명령 추가

        _isFinished = _npcController.Response() && _npcController.EventTrigger(0);
    }

    public bool IsFinished()
    {
        return _isFinished;
    }

    public void End()
    {
        Debug.Log("RoutineCommand : Ended");
    }
}


