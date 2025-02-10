using UnityEngine;

public class RoutineChangeCommand2 : ICommand
{
    private NamedNPCController2 _npcController;
    private bool _isFinished;

    public RoutineChangeCommand2(NamedNPCController2 npc)
    {
        this._npcController = npc;
        this._isFinished = false;
    }
    public void Execute()
    {
        Debug.Log("RoutineChangeCommand2 : Execute");
        if (_npcController.curRoutineIdx != 1)
            _npcController.ChangeRoutine();

        //_npcController.Invoker.AddCommand(new MoveToNavMeshCommand(_npcController));  // 이동 명령 추가

        _isFinished = _npcController.Response() && !_npcController.EventTrigger(0);
    }

    public bool IsFinished()
    {
        return _isFinished;
    }
    public void End()
    {
        Debug.Log("RoutineChangeCommand2 : Ended");
    }
}
