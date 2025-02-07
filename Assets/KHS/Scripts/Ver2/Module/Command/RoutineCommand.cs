using UnityEngine;

public class RoutineCommand : ICommand
{
    private NamedNPCController _npcController;
    private bool _isFinished;

    public RoutineCommand(NamedNPCController npc)
    {
        this._npcController = npc;
        this._isFinished = false;
    }

    public void Execute()
    {
        Debug.Log("RoutineCommand : Executed Call");
        _isFinished = _npcController.Routine();
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

