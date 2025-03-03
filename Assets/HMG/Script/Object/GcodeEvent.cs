using UnityEngine;

public class GcodeEvent : MonoBehaviour
{
    public SphereCollider coll;
    public BoxCollider box;
    public NPC_DoorGaurd doorman;
    public NPCFSM npcFSM;
    public NPCChatTest chat;
    private void OnEnable()
    {
        EventManager.Subscribe(EventManager.GameEventType.GameClear, StartGcode);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(EventManager.GameEventType.GameClear, StartGcode);
    }
    private void StartGcode()
    {
        box.enabled = false;
        doorman.enabled = false;
        npcFSM.enabled = false;
        chat.enabled = false;
        coll.enabled = false;
    }
    


}
