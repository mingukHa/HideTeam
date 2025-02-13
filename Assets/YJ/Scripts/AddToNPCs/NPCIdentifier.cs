using UnityEngine;

public class NPCIdentifier : MonoBehaviour
{
    public Avatar nPCAvatar; // 해당 NPC의 아바타
    public string nPCName; // 해당 NPC 이름
    public string nPCRightHand; // 해당 NPC 오른손 Bone 이름

    private void Start()
    {
        if (nPCAvatar == null)
        {
            Debug.LogWarning($"{gameObject}의 Avatar가 설정되지 않았습니다!");
        }
    }
}