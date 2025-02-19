using UnityEngine;

public class TakingCommand : ICommand
{
    private NPCController npcController;
    private EventManager.GameEventType takeEvent;
    private bool finished = false;

    public TakingCommand(NPCController _npc, EventManager.GameEventType _takeEvent)
    {
        npcController = _npc;
        takeEvent = _takeEvent;
    }

    public void Execute()
    {
        Debug.Log($"{npcController.npcName} 가 {takeEvent}를 기다리는 중!");
        npcController.animator.SetTrigger("Talk");
        EventManager.Subscribe(takeEvent, Triggered);
    }
    public bool IsFinished() => finished;

    public void End()
    {
        npcController.animator.ResetTrigger("Talk");
        EventManager.Unsubscribe(takeEvent, Triggered);
    }
    public void Triggered()
    {
        finished = true;
    }
}
