using UnityEngine;
using System;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public enum GameEventType //�� �κп��� ��� �̺�Ʈ String�� ���� �մϴ�
    {
        //���� ��� ���� �̺�Ʈ
        NULL,     // �ʱ�ȭ�� NULL �̺�Ʈ
        PlayerEnterBank,        // Player ���� ����
        PlayerTalkTeller,         // Teller ��ȣ�ۿ�

        //���� ��Ʈ �κ�
        Carkick,//�ڵ��� ������
        //
        Garbage,//�������� ������

        TellerTalk, //�ڷ����� ���ɱ�
        RichmanAngry, //���� ȭ�� 
        RichmanTalkTeller,  // ���ڰ� �ڷ����� ���ɱ�
        //
        OldManHelp,//���� ������
        OldManoutside,//���� �ȵ�����
        OldManGotoTeller, //������ �ڷ����� ��
        OldManTalkTeller, // ������ �ڷ����� ���ɱ�
        //
        plainclothespoliceTalk, //�纹�������� ���� ��
        plainclothespoliceNoTalk, //�纹�������� ���� �Ȱ�
        //
        policeTalk, //��񿡰� �纹���� ��Ī�ϴ� �ι��� ����
        policeNoTalk, //��񿡰� �纹���� �� ����
        //
        RichToiletKill, //ȭ��ǿ��� ��� �׿��� �� û�Һΰ� ȭ��ǿ� ������ Ÿ�̹�
        RichHide, //��ġ �ϻ� �� ������
        RichNoHide, //��ġ �ϻ� �� ������ ����
        //�÷� a�κ�
        RichKill, //���ڸ� ������ �� û�Һΰ� �κ� ������ �޷����� �̺�Ʈ
        
        RichAngrytimeup,    //Angry1��Ʈ �ð� ���� �� �ٷ� �ߵ�
        //���� ��Ʈ
        //bankemployee, //�ȳ�����ũ 10�ʰ� ����ϴ� �̺�Ʈ
        GameOver, //���� ���� �� �����ϴ� �̺�Ʈ
        //
        NPCKill, //NPC���̸� ȣ��

        RichDead, //��ġ�� �κ񿡼� ������ ȣ��

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
        PlayerToiletOut, //�÷��̾ ��ġ�� ��� ȭ����� ���� ��
        
        ConvEnd,
        RichmanToliet,
        EndingStop, //���� ���µ� īű �� �ϸ� �����°�
        Ending

    }

    private static Dictionary<GameEventType, Action> eventDictionary = new Dictionary<GameEventType, Action>();

    public static void Subscribe(GameEventType eventType, Action listener) //�̺�Ʈ �����ϴ� �Լ� : Eunm����� �̺�Ʈ, ���� �Լ� �̸� �ְ� ����
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

    public static void Unsubscribe(GameEventType eventType, Action listener) //�̺�Ʈ ���� �����ϴ� �Լ� : Eunm����� �̺�Ʈ, ���� �Լ� �̸� �ְ� ����
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

    public static void Trigger(GameEventType eventType) //�̺�Ʈ �߻� Ʈ���� �Լ� : Enum������� ����
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
