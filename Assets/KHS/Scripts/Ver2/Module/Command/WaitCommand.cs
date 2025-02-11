using UnityEngine;

public class WaitCommand : ICommand
{
    private NPCController npcController;
    private float waitTime;
    private float elapsedTime = 0f;
    private bool finished = false;

    public WaitCommand(NPCController _npc, float _time)
    {
        npcController = _npc;
        waitTime = _time;
    }

    public void Execute()
    {
        if(elapsedTime == 0f)
        {
            npcController.animator.SetTrigger("Look");
        }

        elapsedTime += Time.deltaTime;
        if(elapsedTime >= waitTime)
        {
            finished = true;
        }
    }

    public bool IsFinished()
    {
        return finished;
    }

    public void End()
    {
        Debug.Log($"{npcController.npcName}의 루틴 중 대기 종료");
        npcController.animator.ResetTrigger("Look");
    }
}
