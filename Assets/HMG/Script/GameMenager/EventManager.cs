using UnityEngine;
using System;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public enum GameEventType //�� �κп��� ��� �̺�Ʈ ���� �մϴ�
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
            Debug.Log($"{eventType} : �̺�Ʈ �߰���!");
        }
        else
        {
            eventDictionary[eventType] += listener;
            Debug.Log($"{eventType} : �̺�Ʈ �����ڰ� �߰���!");
        }
    }

    public static void Unsubscribe(GameEventType eventType, Action listener)
    {
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] -= listener;
            Debug.Log($"{eventType} : �̺�Ʈ ���� ������!");

            if (eventDictionary[eventType] == null)
            {
                eventDictionary.Remove(eventType);
                Debug.Log($"{eventType} : �̺�Ʈ ������! ������ ����");
            }
        }
    }

    public static void Trigger(GameEventType eventType)
    {
        if (eventDictionary.ContainsKey(eventType))
        {
            Debug.Log($"{eventType} : �̺�Ʈ Ʈ���� �ߵ�!");
            eventDictionary[eventType]?.Invoke();
        }
        else
        {
            Debug.Log($"{eventType} : �̺�Ʈ ����!");
        }
    }
}
