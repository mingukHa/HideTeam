using UnityEngine;

public class NPCChatClick : MonoBehaviour
{
    public string npcID; // Firebase���� ������ NPC ID
    private NPCChatTest npcChatTest;

    private void Start()
    {
        npcChatTest = GetComponent<NPCChatTest>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"NPC {npcID}�� ��ȭ ����...");
            npcChatTest.LoadNPCDialogue(npcID);
        }
    }

}
