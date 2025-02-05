using UnityEngine;

public class PlayerDisguiser : MonoBehaviour
{
    public GameObject defaultCharacter; // �⺻ ĳ����
    public Avatar defaultAvatar; // �⺻ �ƹ�Ÿ
    public GameObject[] characterVariants; // NPC �̸��� ���� ĳ���� ����Ʈ

    private Animator anim;
    private GameObject currentCharacter; // ���� Ȱ��ȭ�� ĳ���� ����

    private void Start()
    {
        anim = GetComponent<Animator>(); // Animator ������Ʈ ��������
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
}
