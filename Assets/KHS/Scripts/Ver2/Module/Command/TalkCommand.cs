using UnityEngine;

public class TalkCommand : ICommand
{
    private NPCController npcController;
    private EventManager.GameEventType talkEvent;
    private float talktime;
    private bool finished = false;
    private float elapsedTime = 0f;

    public TalkCommand(NPCController _npc, EventManager.GameEventType _talkEvent, float _time)
    {
        npcController = _npc;
        talkEvent = _talkEvent;
        talktime = _time;
    }

    public void Execute()
    {
        EventManager.Trigger(talkEvent);
        Debug.Log($"{npcController.npcName} �� {talkEvent}�� Ʈ����!");
        npcController.animator.SetTrigger("Talk");
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= talktime)
        {
            finished = true;
        }

    }
    public bool IsFinished() => finished;

    public void End()
    {
        npcController.animator.ResetTrigger("Talk");
    }

}
