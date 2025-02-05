using UnityEngine;

public class NPCIdentifier : MonoBehaviour
{
    public Avatar nPCAvatar;
    public string nPCName;

    private void Start()
    {
        if (nPCAvatar == null)
        {
            Debug.LogWarning($"{gameObject}의 Avatar가 설정되지 않았습니다!");
        }
    }
}