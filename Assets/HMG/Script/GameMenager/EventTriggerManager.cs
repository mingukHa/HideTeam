using UnityEngine;
using static EventManager;

public class EventTriggerManager : MonoBehaviour
{
    private GameEventType[] npcEvents =
    {
        GameEventType.NULL,
        GameEventType.PlayerEnterBank,
        GameEventType.Carkick,
        GameEventType.Garbage,             
        GameEventType.OldManHelp,
        GameEventType.OldManoutside,
        GameEventType.plainclothespoliceTalk,
        GameEventType.plainclothespoliceNoTalk,
        GameEventType.policeTalk,
        GameEventType.policeNoTalk,
        GameEventType.TellerTalk, 
        GameEventType.RichmanAngry,
        GameEventType.RichAngrytimeup,
        GameEventType.GameOver,
        GameEventType.RichHide,
        GameEventType.RichNoHide,
        GameEventType.NPCKill,
        GameEventType.OldManGotoTeller,
        GameEventType.RichToiletKill,
        GameEventType.OldManOut,
        GameEventType.CleanManTalk,
        GameEventType.CleanManDie,
        GameEventType.GameClear,
        GameEventType.PlayerToiletOut,
        GameEventType.Ending,
        GameEventType.EndingStop,
        GameEventType.CleanManHide
            

    };

    private void OnEnable() //람다식으로 함수 정의
    {//익명 함수를 이벤트 시스템에 등록
        foreach (var npcEvent in npcEvents)
        {
            EventManager.Subscribe(npcEvent, () => TriggerNPCEvent(npcEvent));
        }
    }
   

    private void OnDisable()
    {
        foreach (var npcEvent in npcEvents)
        {
            EventManager.Unsubscribe(npcEvent, () => TriggerNPCEvent(npcEvent));
        }
    }

    private void TriggerNPCEvent(GameEventType eventType)
    {
        Debug.Log($"{eventType} 이벤트 발생!");
    }
}


