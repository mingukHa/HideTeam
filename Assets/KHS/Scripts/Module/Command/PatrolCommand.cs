using UnityEngine;

public class PatrolCommand : ICommand
{
    private GuardController _npcController;
    private bool _isFinished;

    public PatrolCommand(GuardController npc)
    {
        this._npcController = npc;
        this._isFinished = false;
    }

    public void Execute()
    {
        Debug.Log("PatrolCommand : Executed Call");
        _isFinished = _npcController.Patrol() || _npcController.HasDetectedTarget();
    }

    public bool IsFinished()
    {
        return _isFinished;
    }

    public void End()
    {
        Debug.Log("PatrolCommand : Ended");
    }
}
