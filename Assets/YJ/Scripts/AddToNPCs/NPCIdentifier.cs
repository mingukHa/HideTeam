using UnityEngine;

public class NPCIdentifier : MonoBehaviour
{
    public Avatar nPCAvatar; // 해당 NPC의 아바타
    public string nPCName; // 해당 NPC 이름
    public string nPCLeftHandIK; // 해당 NPC 왼손 IK 위치
    public string nPCRightHandIK; // 해당 NPC 오른손 IK 위치

    private void Start()
    {
        if (nPCAvatar == null)
        {
            Debug.LogWarning($"{gameObject}의 Avatar가 설정되지 않았습니다!");
        }
    }
}