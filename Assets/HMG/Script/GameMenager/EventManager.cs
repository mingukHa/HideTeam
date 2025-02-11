using UnityEngine;
using System;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public enum GameEventType //이 부분에서 모든 이벤트 관리 합니다
    {
        Talk,
        Fun,
        Trade,
        QuestComplete
    }

    private static Dictionary<GameEventType, Action> eventDictionary = new Dictionary<GameEventType, Action>();

    public static void Subscribe(GameEventType eventType, Action listener)
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

    public static void Unsubscribe(GameEventType eventType, Action listener)
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

    public static void Trigger(GameEventType eventType)
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
