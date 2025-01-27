using UnityEngine;
using System;

[System.Serializable]
public class EventTrigger
{
    public string triggerName; // �̺�Ʈ �̸�
    public Func<bool> condition; // Ʈ���� ���� ���� (������ �ڵ�� ����)
    public Action effect; // Ʈ���� ���� ȿ��

    public void CheckAndExecute()
    {
        if (condition != null && condition())
        {
            effect?.Invoke();
            Debug.Log($"Event Triggered: {triggerName}");
        }
    }
}
