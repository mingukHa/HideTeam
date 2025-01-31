using UnityEngine;

public class NPCChatClick : MonoBehaviour
{
    public string npcID; // Firebase에서 가져올 NPC ID
    private NPCChatTest npcChatTest;

    private void Start()
    {
        npcChatTest = GetComponent<NPCChatTest>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"NPC {npcID}와 대화 시작...");
            npcChatTest.LoadNPCDialogue(npcID);
        }
    }

}
