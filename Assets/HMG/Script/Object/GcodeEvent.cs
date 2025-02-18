using UnityEngine;

public class GcodeEvent : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Subscribe(EventManager.GameEventType.Conversation5, StartGcode);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(EventManager.GameEventType.Conversation5, StartGcode);
    }
    private void StartGcode()
    {
        box.enabled = false;
        doorman.enabled = false;
        npcFSM.enabled = false;
        chat.enabled = false;    
    }

    public BoxCollider box;
    public NPC_DoorGaurd doorman;
    public NPCFSM npcFSM;
    public NPCChatTest chat;


}
