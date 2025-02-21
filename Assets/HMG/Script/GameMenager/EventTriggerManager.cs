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
        GameEventType.PlayerToiletOut


    };

    private void OnEnable()
    {
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
//구닥다리 쓰레기 코드
//using UnityEngine; 
//using static EventManager;

//public class EventTriggerManager : MonoBehaviour
//{
//    // 이벤트를 배열로 관리하여 코드 가독성 및 유지보수 향상
//    private GameEventType[] npcEvents =
//    {
//        GameEventType.Carkick,
//        GameEventType.Garbage,
//        GameEventType.sweeper,
//        GameEventType.sweeperKill,
//        GameEventType.OldManHelp,
//        GameEventType.OldManoutside,
//        GameEventType.plainclothespoliceTalk,
//        GameEventType.plainclothespoliceNoTalk,
//        GameEventType.policeTalk,
//        GameEventType.policeNoTalk
//    };

//    private void OnEnable()
//    {
//        foreach (var npcEvent in npcEvents)
//        {
//            EventManager.Subscribe(npcEvent, () => TriggerNPCEvent(npcEvent));
//        }
//    }

//    private void OnDisable()
//    {
//        foreach (var npcEvent in npcEvents)
//        {
//            EventManager.Unsubscribe(npcEvent, () => TriggerNPCEvent(npcEvent));
//        }
//    }

//    private void TriggerNPCEvent(GameEventType eventType)
//    {
//        Debug.Log($"{eventType} 트리거 이벤트 발생!");
//    }
//}

