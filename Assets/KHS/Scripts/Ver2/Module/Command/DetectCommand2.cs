using UnityEngine;

public class DetectCommand2 : ICommand
{
    private NPCController _npcController;
    private bool _isFinished;

    public DetectCommand2(NPCController npc)
    {
        _npcController = npc;
        _isFinished = false;
    }

    public void Execute()
    {
        Debug.Log("DetectCommand : Executed Call");
        _isFinished = _npcController.HasDetectedTarget();  // TNPCController���� Ÿ�� ���� ���θ� ������
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