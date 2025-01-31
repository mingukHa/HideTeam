using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Scriptable Objects/Event")]
public class Event : ScriptableObject
{
    public string eventName;
    public void Excute(NPCController npc)
    {
        // 이벤트 로직
    }
}
