using System;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class TalkUIManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// ����
    /// 
    /// 0 �÷��̾� ��ǳ��
    /// 1 �ڷ� ��ǳ��
    /// 2 �õ�� ��ǳ��
    /// 3 ��ġ�� ��ǳ��
    /// 
    /// ����
    /// 
    /// -1 ��� ����
    /// 
    /// </summary>
    public List<GameObject> uiGoList;
    public List<EventManager.GameEventType> eventFlags = new List<EventManager.GameEventType>();
    public List<string> actionNames = new List<string>();
    private Dictionary<GameEventType, Action> eventActions = new Dictionary<GameEventType, Action>();

    public int curBoxIdx = -1;

    private void Start()
    {
        curBoxIdx = -1;
    }

    private void FixedUpdate()
    {
        
    }

    private void OnEnable()
    {
        InitializeEventActions();
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }
    private void InitializeEventActions()
    {
        eventActions.Clear();
        for (int i = 0; i < eventFlags.Count && i < actionNames.Count; i++)
        {
            string methodName = actionNames[i];
            Action action = (Action)Delegate.CreateDelegate(typeof(Action), this, methodName, false);
            if (action != null)
            {
                eventActions[eventFlags[i]] = action;
            }
            else
            {
                Debug.LogWarning($"{methodName} �޼��带 ã�� �� �����ϴ�! TalkUIManager�� ���ǵǾ� �־�� �մϴ�.");
            }
        }
    }
    // �̺�Ʈ ���
    private void SubscribeEvents()
    {
        foreach (var kvp in eventActions)
        {
            EventManager.Subscribe(kvp.Key, kvp.Value);
        }
    }

    // �̺�Ʈ ����
    private void UnsubscribeEvents()
    {
        foreach (var kvp in eventActions)
        {
            EventManager.Unsubscribe(kvp.Key, kvp.Value);
        }
    }

    public void RichTalk()
    {
        curBoxIdx = 3;
        SwitchBox();
    }
    public void OldManTalk()
    {
        curBoxIdx = 2;
        SwitchBox();
    }
    public void PlayerTalk()
    {
        curBoxIdx = 0;
        SwitchBox();
    }
    public void TellerTalk()
    {
        curBoxIdx = 1;
        SwitchBox();
    }
    public void GuardTalk()
    {

    }
    public void CleanerTalk()
    {

    }
    public void TalkReset()
    {
        curBoxIdx = -1;
        SwitchBox();
    }

    public void SwitchBox()
    {
        for (int i = 0; i < uiGoList.Count; ++i)
        {
            if (i == curBoxIdx)
                uiGoList[i].gameObject.SetActive(true);
            else
                uiGoList[i].gameObject.SetActive(false);
        }
    }
}
