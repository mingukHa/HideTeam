using UnityEngine;

public class MoveCommand : ICommand
{
    private TNPCController _npcController;
    private Vector3 _targetPoint;
    private bool _isFinished;

    public MoveCommand(TNPCController npc, Vector3 targetPoint)
    {
        this._npcController = npc;
        this._targetPoint = targetPoint;
        this._isFinished = false;
    }

    public void Execute()
    {
        Debug.Log("MoveCommand : Executed Call");
        _npcController.MoveToTarget(_targetPoint);
        _isFinished = true;
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
