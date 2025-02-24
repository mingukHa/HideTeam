using UnityEngine;
using System;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public enum GameEventType //이 부분에서 모든 이벤트 String을 관리 합니다
    {
        //공통 상시 동작 이벤트
        NULL,     // 초기화용 NULL 이벤트
        PlayerEnterBank,        // Player 은행 진입
        PlayerTalkTeller,         // Teller 상호작용

        //공통 루트 부분
        Carkick,//자동차 발차기
        //
        Garbage,//쓰레기장 헤집기

        TellerTalk, //텔러에게 말걸기
        RichmanAngry, //부자 화남 
        RichmanTalkTeller,  // 부자가 텔러에게 말걸기
        //
        OldManHelp,//노인 도와줌
        OldManoutside,//노인 안도와줌
        OldManGotoTeller, //노인이 텔러에게 감
        OldManTalkTeller, // 노인이 텔러에게 말걸기
        //
        plainclothespoliceTalk, //사복경찰에게 말을 검
        plainclothespoliceNoTalk, //사복경찰에게 말을 안검
        //
        policeTalk, //경비에게 사복경찰 지칭하는 인물을 말함
        policeNoTalk, //경비에게 사복경찰 말 안함
        //
        RichToiletKill, //화장실에서 경비를 죽였을 때 청소부가 화장실에 들어오는 타이밍
        RichHide, //리치 암살 후 숨겼음
        RichNoHide, //리치 암살 후 숨기지 못함
        //플랜 a부분
        RichKill, //부자를 제압할 때 청소부가 로비에 있으면 달려가는 이벤트
        
        RichAngrytimeup,    //Angry1루트 시간 종료 후 바로 발동
        //메인 루트
        //bankemployee, //안내데스크 10초간 대기하는 이벤트
        GameOver, //게임 오버 시 동작하는 이벤트
        //
        NPCKill, //NPC죽이면 호출

        RichDead, //리치가 로비에서 죽으면 호출

        Conversation1,
        Conversation2,
        Conversation3,
        Conversation4,
        Conversation5,
        Conversation6,

        OldManOut,

        RichManTalkUI,
        OldManTalkUI,
        PlayerTalkUI,
        TellerTalkUI,
        GuardTalkUI,
        CleanerTalkUI,
        ResetTalkUI,

        ConvStart,
        
        CleanManTalk,
        CleanManDie,

        GameClear,
        PlayerToiletOut, //플레이어가 리치맨 잡고 화장실을 나올 때
        
        ConvEnd,
        RichmanToliet,
        EndingStop, //엔딩 가는데 카킥 안 하면 터지는거
        Ending

    }

    private static Dictionary<GameEventType, Action> eventDictionary = new Dictionary<GameEventType, Action>();

    public static void Subscribe(GameEventType eventType, Action listener) //이벤트 구독하는 함수 : Eunm방식의 이벤트, 실행 함수 이름 넣고 실행
    {
        if (!eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] = listener;
            Debug.Log($"{eventType} : 이벤트 추가됨!");
        }
        else
        {
            eventDictionary[eventType] += listener;
            Debug.Log($"{eventType} : 이벤트 구독자가 추가됨!");
        }
    }

    public static void Unsubscribe(GameEventType eventType, Action listener) //이벤트 구독 해제하는 함수 : Eunm방식의 이벤트, 실행 함수 이름 넣고 실행
    {
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] -= listener;
            Debug.Log($"{eventType} : 이벤트 구독 해제됨!");

            if (eventDictionary[eventType] == null)
            {
                eventDictionary.Remove(eventType);
                Debug.Log($"{eventType} : 이벤트 삭제됨! 구독자 없음");
            }
        }
    }

    public static void Trigger(GameEventType eventType) //이벤트 발생 트리거 함수 : Enum방식으로 실행
    {
        if (eventDictionary.ContainsKey(eventType))
        {
            Debug.Log($"{eventType} : 이벤트 트리거 발동!");
            eventDictionary[eventType]?.Invoke();
        }
        else
        {
            Debug.Log($"{eventType} : 이벤트 없음!");
        }
    }
}
