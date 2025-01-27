using UnityEngine;

[System.Serializable]
public class AIModule
{
    public AITriggerCondition triggerCondition; // Ʈ���� ����
    public string moduleName; // ��� �̸� (Animator�� �ൿ ���� �ڵ� ����)

    public void Execute()
    {
        Debug.Log($"Executing AI Module: {moduleName}");
        // AI ��� ���� ���� �߰� (e.g., Animator Ʈ����)
    }
}
