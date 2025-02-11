using UnityEngine;
using static EventManager;

public class EventTriggerManager : MonoBehaviour
{
    private GameEventType npcEventTalk = GameEventType.Talk; // Enum 사용
    private GameEventType npcEventFun = GameEventType.Fun;

    private void OnEnable()
    {
        EventManager.Subscribe(npcEventTalk, TriggerNPCTalk);
        EventManager.Subscribe(npcEventFun, TriggerNPCFun);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(npcEventTalk, TriggerNPCTalk);
        EventManager.Unsubscribe(npcEventFun, TriggerNPCFun);
    }

    private void TriggerNPCTalk()
    {
        Debug.Log(" 이야기 트리거 이벤트");
    }

    private void TriggerNPCFun()
    {
        Debug.Log(" 웃음 트리거 이벤트");
    }
}
