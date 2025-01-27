using UnityEngine;
using System;

[System.Serializable]
public class EventTrigger
{
    public string triggerName; // 이벤트 이름
    public Func<bool> condition; // 트리거 실행 조건 (조건은 코드로 설정)
    public Action effect; // 트리거 실행 효과

    public void CheckAndExecute()
    {
        if (condition != null && condition())
        {
            effect?.Invoke();
            Debug.Log($"Event Triggered: {triggerName}");
        }
    }
}
