using UnityEngine;

public class NPCIdentifier : MonoBehaviour
{
    public Avatar nPCAvatar;
    public string nPCName;

    private void Start()
    {
        if (nPCAvatar == null)
        {
            Debug.LogWarning($"{gameObject}�� Avatar�� �������� �ʾҽ��ϴ�!");
        }
    }
}