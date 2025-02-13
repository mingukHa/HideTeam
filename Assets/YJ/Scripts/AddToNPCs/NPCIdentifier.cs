using UnityEngine;

public class NPCIdentifier : MonoBehaviour
{
    public Avatar nPCAvatar; // �ش� NPC�� �ƹ�Ÿ
    public string nPCName; // �ش� NPC �̸�
    public string nPCLeftHandIK; // �ش� NPC �޼� IK ��ġ
    public string nPCRightHandIK; // �ش� NPC ������ IK ��ġ

    private void Start()
    {
        if (nPCAvatar == null)
        {
            Debug.LogWarning($"{gameObject}�� Avatar�� �������� �ʾҽ��ϴ�!");
        }
    }
}