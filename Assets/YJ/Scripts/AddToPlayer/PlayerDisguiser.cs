using UnityEngine;
using UnityEngine.UI;

public class PlayerDisguiser : MonoBehaviour
{
    public GameObject defaultCharacter; // �⺻ ĳ����
    public Avatar defaultAvatar; // �⺻ �ƹ�Ÿ
    //public Transform defaultLeftHandIKTarget; // �⺻ �޼� IK Ÿ��
    //public Transform defaultRightHandIKTarget; // �⺻ ������ IK Ÿ��
    public GameObject[] characterVariants; // NPC �̸��� ���� ĳ���� ����Ʈ

    private Animator anim;
    private GameObject currentCharacter; // ���� Ȱ��ȭ�� ĳ���� ����
    private RagdollGrabber ragdollGrabber;


    private void Start()
    {
        anim = GetComponent<Animator>(); // Animator ������Ʈ ��������
        ragdollGrabber = GetComponent<RagdollGrabber>(); // RagdollGrabbber Ŭ���� ��������
        currentCharacter = defaultCharacter; // �ʱ� ���¿��� �⺻ ĳ���Ͱ� Ȱ��ȭ��
    }

    public void ChangeAppearance(NPCIdentifier npc)
    {
        if (npc == null) return;

        // 1. ���� ĳ���� ��Ȱ��ȭ
        if (currentCharacter != null)
        {
            currentCharacter.SetActive(false);
        }

        // 2. NPC �̸��� �´� ĳ���� Ȱ��ȭ
        GameObject newCharacter = FindCharacterByName(npc.nPCName);
        if (newCharacter != null)
        {
            newCharacter.SetActive(true);
            currentCharacter = newCharacter; // Ȱ��ȭ�� ĳ���͸� ����
        }
        else
        {
            Debug.LogWarning($"{npc.nPCName}�� �ش��ϴ� ĳ���͸� ã�� �� �����ϴ�!");
            // ĳ���͸� ã�� ���ϸ� �⺻ ĳ���� ����
            if (defaultCharacter != null)
            {
                defaultCharacter.SetActive(true);
                currentCharacter = defaultCharacter;
            }
        }

        // 3. PlayerHolder�� Avatar ����
        if (anim != null && npc.nPCAvatar != null)
        {
            anim.avatar = npc.nPCAvatar;
        }
        else
        {
            Debug.LogWarning("Animator �Ǵ� NPC Avatar�� �������� �ʾҽ��ϴ�.");
        }

        //// 4. Ragdoll Grabber�� Hand IK Target Ȱ��ȭ�� NPC�� ��ü
        //if (npc.nPCLeftHandIK != null && npc.nPCRightHandIK != null && newCharacter != null)
        //{
        //    Transform leftHand = FindDeepChild(newCharacter.transform, npc.nPCLeftHandIK);
        //    Transform rightHand = FindDeepChild(newCharacter.transform, npc.nPCRightHandIK);
        //    if (leftHand != null && rightHand != null)
        //    {
        //        ragdollGrabber.leftHandIKTarget = leftHand;
        //        ragdollGrabber.rightHandIKTarget = rightHand;
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"{npc.nPCLeftHandIK} ������Ʈ�� ã�� �� �����ϴ�.");
        //        Debug.LogWarning($"{npc.nPCRightHandIK} ������Ʈ�� ã�� �� �����ϴ�.");
        //    }
        //}
    }

    public void ResetToDefaultCharacter()
    {
        if (currentCharacter != null)
        {
            currentCharacter.SetActive(false); // ���� ������ ĳ���� ��Ȱ��ȭ
        }

        if (defaultCharacter != null)
        {
            defaultCharacter.SetActive(true); // �⺻ ĳ���� Ȱ��ȭ
            currentCharacter = defaultCharacter;
        }

        if (anim != null)
        {
            anim.avatar = defaultAvatar; // �⺻ Avatar�� ����
        }

        //ragdollGrabber.leftHandIKTarget = defaultLeftHandIKTarget;
        //ragdollGrabber.rightHandIKTarget = defaultRightHandIKTarget;
    }

    private GameObject FindCharacterByName(string npcName)
    {
        foreach (GameObject character in characterVariants)
        {
            if (character.name == npcName)
            {
                return character;
            }
        }
        return null;
    }

    //// ��������� �ڽ� ������Ʈ ã�� (������ Ž����)
    //private Transform FindDeepChild(Transform parent, string childName)
    //{
    //    foreach (Transform child in parent)
    //    {
    //        if (child.name == childName)
    //        {
    //            return child;
    //        }
    //        Transform found = FindDeepChild(child, childName);
    //        if (found != null)
    //        {
    //            return found;
    //        }
    //    }
    //    return null;
    //}
}
