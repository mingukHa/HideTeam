using UnityEngine;

[System.Serializable]
public class AIModule
{
    public AITriggerCondition triggerCondition; // 트리거 조건
    public string moduleName; // 모듈 이름 (Animator나 행동 설계 코드 참조)

    public void Execute()
    {
        Debug.Log($"Executing AI Module: {moduleName}");
        // AI 모듈 실행 로직 추가 (e.g., Animator 트리거)
    }
}
