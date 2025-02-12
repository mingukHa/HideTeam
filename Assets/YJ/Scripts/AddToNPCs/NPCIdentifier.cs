using UnityEngine;

public class NPCIdentifier : MonoBehaviour
{
    public Avatar nPCAvatar; // �ش� NPC�� �ƹ�Ÿ
    public string nPCName; // �ش� NPC �̸�
    public string nPCRightHand; // �ش� NPC ������ Bone �̸�

    private void Start()
    {
        if (nPCAvatar == null)
        {
            Debug.LogWarning($"{gameObject}�� Avatar�� �������� �ʾҽ��ϴ�!");
        }
    }
}