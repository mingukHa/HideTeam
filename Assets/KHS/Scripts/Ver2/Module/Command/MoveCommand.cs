using UnityEngine;

public class MoveCommand : ICommand
{
    private TNPCController _npcController;
    private bool _isFinished;

    public MoveCommand(TNPCController npc)
    {
        this._npcController = npc;
        this._isFinished = false;
    }

    public void Execute()
    {
        Debug.Log("MoveCommand : Executed Call");
        _isFinished = _npcController.MoveToTarget(_npcController._target);
    }

    public bool IsFinished()
    {
        return _isFinished;
    }

    public void End()
    {
        Debug.Log("MoveCommand : Ended");
    }
}
