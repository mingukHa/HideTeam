using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI Module", menuName = "Scriptable Objects/AI Module")]
public class AIModule : ScriptableObject
{
    public string moduleName;
    public List<string> behaviors;
    public void ExecuteBehavior(NPCController npc)
    {
        // Ư�� ���� ���� ����
    }
}
