using UnityEngine;

public class DetectCommand : ICommand
{
    private TNPCController _npcController;
    private bool _isFinished;

    public DetectCommand(TNPCController npc)
    {
        _npcController = npc;
        _isFinished = false;
    }

    public void Execute()
    {
        Debug.Log("DetectCommand : Executed Call");
        _isFinished = _npcController.HasDetectedTarget();  // TNPCController에서 타겟 감지 여부를 가져옴
    }

    public bool IsFinished()
    {
        return _isFinished;
    }

    public void End()
    {
        Debug.Log("DetectCommand : Ended");
    }
}