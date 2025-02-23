using UnityEngine;

public class TellCommand : ICommand
{
    private NPCController npcController;
    private EventManager.GameEventType talkEvent;
    private float talktime;
    private bool finished = false;
    private float elapsedTime = 0f;

    public TellCommand(NPCController _npc, EventManager.GameEventType _talkEvent, float _time)
    {
        npcController = _npc;
        talkEvent = _talkEvent;
        talktime = _time;
    }

    public void Execute()
    {
        npcController.animator.SetTrigger("Tell");
        Debug.Log($"{npcController.npcName} 가 {talkEvent}를 트리거!");
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= talktime)
        {
            EventManager.Trigger(talkEvent);
            finished = true;
        }

    }
    public bool IsFinished() => finished;

    public void End()
    {
        npcController.animator.ResetTrigger("Tell");
    }

}
